using back_cw20_integration.Configuration.PrometheusMetrics.Middleware;
using back_cw20_integration.Configuration.PrometheusMetrics.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace back_cw20_integration.Configuration.PrometheusMetrics
{
    public static class PrometheusExtensions
    {
        public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services) 
        {
            services.AddSingleton<ServiceMetrics>();

            return services;
        }

        public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app) 
        {
            // 1. Middleware para métricas HTTP básicas (código de estado, duración, etc.)
            app.UseHttpMetrics();

            // 2. Middleware personalizado para monitoreo detallado de recursos
            app.UseMiddleware<ResourceMonitoringMiddleware>();

            return app;
        }
    }
}
