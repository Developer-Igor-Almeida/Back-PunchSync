using MediatR;
using PunchSync.Domain.Common;
using PunchSync.Application.UseCases.Auth.Common;

namespace PunchSync.Application.UseCases.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;
