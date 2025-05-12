using back_cw20_integration.Application.PowerBI.Interfaces;
using back_cw20_integration.Infrastructure.Cache;
using back_cw20_integration.Infrastructure.Mediator;
using back_cw20_integration.Infrastructure.Persistence;
using back_cw20_integration.Infrastructure.PowerBI;
using back_cw20_integration.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace back_cw20_integration.Infrastructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddRedisExtension(configuration);

            services.AddPersistenceExtension(configuration);

            services.AddRepositoryExtension();

            // Infraestructure Services

            services.AddScoped<IPowerBIService, PowerBIService>();

            services.AddCustomMediatorExtension();

            return services;
        }
    }
}