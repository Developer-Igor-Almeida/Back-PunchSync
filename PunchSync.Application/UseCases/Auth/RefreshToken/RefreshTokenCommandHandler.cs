using MediatR;
using PunchSync.Application.Interfaces;
using PunchSync.Application.UseCases.Auth.Common;
using PunchSync.Domain.Common;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;

namespace PunchSync.Application.UseCases.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null)
            return Result<AuthResponse>.Failure("Refresh token inválido.", "INVALID_REFRESH_TOKEN");

        var currentToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);
        if (currentToken is null || !currentToken.IsActive)
            return Result<AuthResponse>.Failure("Refresh token expirado ou revogado.", "REFRESH_TOKEN_INACTIVE");

        // Rotação: revoga o token usado e emite um novo
        currentToken.Revoke();

        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString(), user.TenantId);
        var newRefreshTokenValue = jwtService.GenerateRefreshToken();
        var newRefreshToken = user.AddRefreshToken(newRefreshTokenValue, jwtService.GetRefreshTokenExpiry());

        // A revogação do token atual (Modified) é detectada pelo change tracker;
        // o novo token precisa do Add explícito para virar INSERT.
        userRepository.AddRefreshToken(newRefreshToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var response = new AuthResponse(
            user.Id, user.Name, user.Email, user.Role.ToString(),
            accessToken.Token, newRefreshTokenValue, accessToken.ExpiresAt);

        return Result<AuthResponse>.Success(response);
    }
}
