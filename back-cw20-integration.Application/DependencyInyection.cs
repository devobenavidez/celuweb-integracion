using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace back_cw20_integration.Application
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
