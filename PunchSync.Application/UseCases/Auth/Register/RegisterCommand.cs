using MediatR;
using PunchSync.Domain.Common;
using PunchSync.Application.UseCases.Auth.Common;

namespace PunchSync.Application.UseCases.Auth.Register;

public sealed record RegisterCommand(
    string Name,
    string Email,
    string Password,
    Guid TenantId) : IRequest<Result<AuthResponse>>;
