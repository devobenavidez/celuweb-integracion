using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace back_cw20_integration.Infrastructure.Repositories
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositoryExtension(this IServiceCollection services) 
        {
            // Registrar todos los repositorios de forma dinámica con reflection
            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                            t.GetInterfaces().Any(i => i.Name.Contains("Repository")));


            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces()
                    .FirstOrDefault(i => i.Name.Contains("Repository"));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repositoryType);
                }
            }

            return services;
        }
    }
}
