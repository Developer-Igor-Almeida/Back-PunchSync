using MediatR;
using PunchSync.Domain.Common;
using PunchSync.Domain.Entities;
using PunchSync.Domain.Interfaces;
using PunchSync.Domain.Interfaces.Repositories;

namespace PunchSync.Application.UseCases.Gyms.CreateGym;

public sealed class CreateGymCommandHandler(
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateGymCommand, Result<CreateGymResponse>>
{
    public async Task<Result<CreateGymResponse>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var slug = request.Slug.Trim().ToLowerInvariant();

        if (await gymRepository.SlugExistsAsync(slug, cancellationToken))
            return Result<CreateGymResponse>.Failure("Slug já está em uso.", "SLUG_ALREADY_EXISTS");

        var gym = new Gym(request.Name.Trim(), slug);

        await gymRepository.AddAsync(gym, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result<CreateGymResponse>.Success(new CreateGymResponse(gym.Id, gym.Name, gym.Slug));
    }
}
