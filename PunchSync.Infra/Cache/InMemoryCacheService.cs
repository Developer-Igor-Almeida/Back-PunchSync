using Microsoft.Extensions.Caching.Memory;
using PunchSync.Application.Interfaces;

namespace PunchSync.Infra.Cache;

// Implementação de cache em memória — usada em desenvolvimento/estudos para
// evitar a dependência de um Redis rodando. Em produção, troca-se o provider
// para Redis via configuração "Cache:Provider": "Redis" (ver DependencyInjection).
public class InMemoryCacheService(IMemoryCache cache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        => Task.FromResult(cache.TryGetValue(key, out T? value) ? value : default);

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        cache.Set(key, value, expiry ?? TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(key, out T? cached) && cached is not null)
            return cached;

        var value = await factory();
        cache.Set(key, value, expiry ?? TimeSpan.FromMinutes(10));
        return value;
    }
}
