using PunchSync.Domain.Entities;

namespace PunchSync.Domain.Interfaces.Repositories;

public interface IGymRepository : IBaseRepository<Gym>
{
    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
}
