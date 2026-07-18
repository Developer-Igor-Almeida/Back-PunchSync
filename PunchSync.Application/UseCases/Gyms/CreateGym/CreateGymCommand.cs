using MediatR;
using PunchSync.Domain.Common;

namespace PunchSync.Application.UseCases.Gyms.CreateGym;

public sealed record CreateGymResponse(Guid Id, string Name, string Slug);

public sealed record CreateGymCommand(string Name, string Slug) : IRequest<Result<CreateGymResponse>>;
