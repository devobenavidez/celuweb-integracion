using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Infrastructure.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace back_cw20_integration.Infrastructure.Mediator
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddCustomMediatorExtension(this IServiceCollection services) 
        {
            services.AddScoped<IMediator, MediatorService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Registrar todos los handlers desde la capa de Application
            var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "back-cw20-integration.Application");

            if (applicationAssembly != null)
                RegisterHandlers(services, applicationAssembly);

            return services;
        }
        private static void RegisterHandlers(IServiceCollection services, Assembly applicationAssembly)
        {
            var handlerTypes = applicationAssembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface &&
                           t.GetInterfaces().Any(i =>
                               i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                // Encontrar la interfaz IRequestHandler que implementa
                var handlerInterface = handlerType.GetInterfaces()
                    .First(i => i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                // Registrar el handler
                services.AddTransient(handlerInterface, handlerType);
            }
        }
    }
}
