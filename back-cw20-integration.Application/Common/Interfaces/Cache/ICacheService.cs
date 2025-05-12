namespace back_cw20_integration.Application.Common.Interfaces.Cache
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<T?> GetByRefreshCacheAsync<T>(string key, bool refreshCache);
    }
}
