using back_cw20_integration.Application.Common.Interfaces.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace back_cw20_integration.Infrastructure.Persistence
{
    public static class PersistenceExtension
    {
        public static IServiceCollection AddPersistenceExtension(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton<IDbConnectionFactory>(provider =>
                new SqlConnectionFactory(configuration));

            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

            return services;
        }
    }
}
