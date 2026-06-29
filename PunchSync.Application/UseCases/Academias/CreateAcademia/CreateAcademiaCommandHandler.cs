using MediatR;
using PunchSync.Domain.Common;
using PunchSync.Domain.Entities;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;

namespace PunchSync.Application.UseCases.Academias.CreateAcademia;

public sealed class CreateAcademiaCommandHandler(
    IAcademiaRepository academiaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAcademiaCommand, Result<CreateAcademiaResponse>>
{
    public async Task<Result<CreateAcademiaResponse>> Handle(CreateAcademiaCommand request, CancellationToken cancellationToken)
    {
        var slug = request.Slug.Trim().ToLowerInvariant();

        if (await academiaRepository.SlugExistsAsync(slug, cancellationToken))
            return Result<CreateAcademiaResponse>.Failure("Slug já está em uso.", "SLUG_ALREADY_EXISTS");

        var academia = new Academia(request.Name.Trim(), slug);

        await academiaRepository.AddAsync(academia, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<CreateAcademiaResponse>.Success(new CreateAcademiaResponse(academia.Id, academia.Name, academia.Slug));
    }
}
