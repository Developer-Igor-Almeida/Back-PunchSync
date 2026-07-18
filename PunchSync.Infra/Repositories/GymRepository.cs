using Microsoft.EntityFrameworkCore;
using PunchSync.Domain.Entities;
using PunchSync.Domain.Interfaces.Repositories;
using PunchSync.Infra.Data;

namespace PunchSync.Infra.Repositories;

public class GymRepository(AppDbContext context) : BaseRepository<Gym>(context), IGymRepository
{
    public async Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(g => g.Slug == slug, cancellationToken);
}
