using MediatR;
using PunchSync.Domain.Common;
using PunchSync.Application.UseCases.Auth.Common;

namespace PunchSync.Application.UseCases.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResponse>>;
