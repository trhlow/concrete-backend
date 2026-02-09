public interface ITwoFactorCache
{
    Task SetAsync(string sessionId, Guid userId, TimeSpan ttl);
    Task<Guid?> GetAsync(string sessionId);
    Task RemoveAsync(string sessionId);
}
