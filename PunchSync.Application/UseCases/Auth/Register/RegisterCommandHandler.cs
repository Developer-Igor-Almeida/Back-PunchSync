using MediatR;
using PunchSync.Application.Interfaces;
using PunchSync.Application.UseCases.Auth.Common;
using PunchSync.Domain.Common;
using PunchSync.Domain.Entities;
using PunchSync.Domain.Enums;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;

namespace PunchSync.Application.UseCases.Auth.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IGymRepository gymRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (gym is null)
            return Result<AuthResponse>.Failure("Academia não encontrada.", "GYM_NOT_FOUND");

        if (await userRepository.EmailExistsAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken))
            return Result<AuthResponse>.Failure("E-mail já cadastrado.", "EMAIL_ALREADY_EXISTS");

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = new User(
            request.Name.Trim(),
            request.Email.Trim().ToLowerInvariant(),
            passwordHash,
            UserRole.Aluno,
            request.TenantId);

        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString(), user.TenantId);
        var refreshTokenValue = jwtService.GenerateRefreshToken();
        user.AddRefreshToken(refreshTokenValue, jwtService.GetRefreshTokenExpiry());

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var response = new AuthResponse(
            user.Id, user.Name, user.Email, user.Role.ToString(),
            accessToken.Token, refreshTokenValue, accessToken.ExpiresAt);

        return Result<AuthResponse>.Success(response);
    }
}
