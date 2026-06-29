using MediatR;
using PunchSync.Domain.Common;

namespace PunchSync.Application.UseCases.Academias.CreateAcademia;

public sealed record CreateAcademiaResponse(Guid Id, string Name, string Slug);

public sealed record CreateAcademiaCommand(string Name, string Slug) : IRequest<Result<CreateAcademiaResponse>>;
