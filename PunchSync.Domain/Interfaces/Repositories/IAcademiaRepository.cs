using PunchSync.Domain.Entities;

namespace PunchSync.Domain.Interfaces.Repositories;

public interface IAcademiaRepository : IBaseRepository<Academia>
{
    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
}
