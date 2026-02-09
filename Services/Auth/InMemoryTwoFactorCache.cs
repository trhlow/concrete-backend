using Microsoft.Extensions.Caching.Memory;

namespace Concrete.Api.Services.Auth;

public class InMemoryTwoFactorCache : ITwoFactorCache
{
    private readonly IMemoryCache _cache;

    public InMemoryTwoFactorCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task SetAsync(string sessionId, Guid userId, TimeSpan ttl)
    {
        _cache.Set($"2fa:{sessionId}", userId, ttl);
        return Task.CompletedTask;
    }

    public Task<Guid?> GetAsync(string sessionId)
    {
        if (_cache.TryGetValue($"2fa:{sessionId}", out Guid userId))
            return Task.FromResult<Guid?>(userId);

        return Task.FromResult<Guid?>(null);
    }

    public Task RemoveAsync(string sessionId)
    {
        _cache.Remove($"2fa:{sessionId}");
        return Task.CompletedTask;
    }
}
