using MediatR;
using PunchSync.Application.Interfaces;
using PunchSync.Application.UseCases.Auth.Common;
using PunchSync.Domain.Common;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;

namespace PunchSync.Application.UseCases.Auth.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("E-mail ou senha inválidos.", "INVALID_CREDENTIALS");

        if (!user.IsActive)
            return Result<AuthResponse>.Failure("Usuário inativo.", "USER_INACTIVE");

        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString(), user.TenantId);
        var refreshTokenValue = jwtService.GenerateRefreshToken();
        var refreshToken = user.AddRefreshToken(refreshTokenValue, jwtService.GetRefreshTokenExpiry());

        // Add explícito garante INSERT do novo token (a coleção do user não é
        // carregada no login e a PK Guid é gerada pela app).
        userRepository.AddRefreshToken(refreshToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var response = new AuthResponse(
            user.Id, user.Name, user.Email, user.Role.ToString(),
            accessToken.Token, refreshTokenValue, accessToken.ExpiresAt);

        return Result<AuthResponse>.Success(response);
    }
}
