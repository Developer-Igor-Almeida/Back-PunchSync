using PunchSync.Domain.Entities;

namespace PunchSync.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    // Marca o refresh token explicitamente como nova entidade (INSERT). Necessário
    // porque a coleção de tokens do User nem sempre é carregada, e a PK Guid é
    // gerada pela aplicação — sem o Add explícito, o EF tenta UPDATE.
    void AddRefreshToken(RefreshToken token);
}
