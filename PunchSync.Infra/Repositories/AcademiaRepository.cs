using Microsoft.EntityFrameworkCore;
using PunchSync.Domain.Entities;
using PunchSync.Domain.Interfaces.Repositories;
using PunchSync.Infra.Data;

namespace PunchSync.Infra.Repositories;

public class AcademiaRepository(AppDbContext context) : BaseRepository<Academia>(context), IAcademiaRepository
{
    public async Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(a => a.Slug == slug, cancellationToken);
}
