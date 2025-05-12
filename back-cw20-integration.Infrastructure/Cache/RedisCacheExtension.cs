using back_cw20_integration.Application.Common.Interfaces.Cache;
using back_cw20_integration.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace back_cw20_integration.Infrastructure.Cache
{
    public static class RedisCacheExtension
    {
        public static IServiceCollection AddRedisExtension (this IServiceCollection services, IConfiguration configuration) 
        {
            // Configurar environment
            services.Configure<EnvironmentSettings>(
            configuration.GetSection("EnvironmentSettings"));

            // Configuración de Redis
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                ConfigurationOptions redisConfiguration = ConfigurationOptions.Parse(
                    configuration.GetConnectionString("Redis") ?? string.Empty, true);
                redisConfiguration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });

            services.AddSingleton<ISerializer, JsonSerializerService>();
            services.AddSingleton<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
