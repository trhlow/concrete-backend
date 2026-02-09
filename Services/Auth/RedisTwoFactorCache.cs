using StackExchange.Redis;

namespace Concrete.Api.Services.Auth;

public class RedisTwoFactorCache : ITwoFactorCache
{
    private readonly IDatabase _db;

    public RedisTwoFactorCache(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task SetAsync(string sessionId, Guid userId, TimeSpan ttl)
    {
        await _db.StringSetAsync(
            $"2fa:{sessionId}",
            userId.ToString(),
            ttl
        );
    }

    public async Task<Guid?> GetAsync(string sessionId)
    {
        var value = await _db.StringGetAsync($"2fa:{sessionId}");
        return value.HasValue ? Guid.Parse(value.ToString()) : null;
    }

    public async Task RemoveAsync(string sessionId)
    {
        await _db.KeyDeleteAsync($"2fa:{sessionId}");
    }
}
