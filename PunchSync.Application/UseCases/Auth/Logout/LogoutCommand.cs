using MediatR;
using PunchSync.Domain.Common;

namespace PunchSync.Application.UseCases.Auth.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<Result>;
