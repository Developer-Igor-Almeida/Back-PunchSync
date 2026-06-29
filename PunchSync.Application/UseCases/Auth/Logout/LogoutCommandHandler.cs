using MediatR;
using PunchSync.Domain.Common;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;

namespace PunchSync.Application.UseCases.Auth.Logout;

public sealed class LogoutCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        var token = user?.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);

        if (token is null)
            return Result.Success(); // idempotente: token inexistente já é "deslogado"

        // user vem rastreado (Include dos RefreshTokens); a revogação é detectada
        // automaticamente pelo change tracker — não precisa de Update().
        token.Revoke();
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
