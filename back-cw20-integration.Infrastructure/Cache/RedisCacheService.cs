using back_cw20_integration.Application.Common.Interfaces.Cache;
using back_cw20_integration.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace back_cw20_integration.Infrastructure.Cache
{
    public class RedisCacheService(IConnectionMultiplexer redis
        , ILogger<RedisCacheService> logger
        , ISerializer serializer
        , IOptions<EnvironmentSettings> environment) : ICacheService
    {
        private readonly IConnectionMultiplexer _redis = redis;
        private readonly ILogger<RedisCacheService> _logger = logger;
        private readonly ISerializer _serializer = serializer;
        private readonly EnvironmentSettings _environment = environment.Value;
        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            string finalKey = string.Concat(key, '_', _environment.Environment);

            try
            {
                var value = await db.StringGetAsync(finalKey);
                if (value.IsNull)
                    return default;

                return _serializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de Redis para la clave {Key}", finalKey);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            var db = _redis.GetDatabase();
            string finalKey = string.Concat(key, '_', _environment.Environment);

            try
            {
                var serialized = _serializer.Serialize(value);
                if (expirationTime.HasValue)
                {
                    await db.StringSetAsync(finalKey, serialized, expirationTime);
                }
                else
                {
                    await db.StringSetAsync(finalKey, serialized);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar datos en Redis para la clave {Key}", finalKey);
            }
        }

        public async Task RemoveAsync(string key)
        {
            var db = _redis.GetDatabase();
            string finalKey = string.Concat(key, '_', _environment.Environment);

            try
            {
                await db.KeyDeleteAsync(finalKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar datos de Redis para la clave {Key}", finalKey);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var db = _redis.GetDatabase();
            string finalKey = string.Concat(key, '_', _environment.Environment);

            try
            {
                return await db.KeyExistsAsync(finalKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de clave {Key} en Redis", finalKey);
                return false;
            }
        }

        public async Task<T?> GetByRefreshCacheAsync<T>(string key, bool refreshCache)
        {
            string finalKey = string.Concat(key, '_', _environment.Environment);

            if (refreshCache) 
            { 
                await RemoveAsync(finalKey);
                return default;
            }

            return await GetAsync<T>(finalKey);
        }
    }
}
