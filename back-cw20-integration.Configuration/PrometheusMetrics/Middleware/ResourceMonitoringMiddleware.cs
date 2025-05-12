using back_cw20_integration.Configuration.PrometheusMetrics.Service;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace back_cw20_integration.Configuration.PrometheusMetrics.Middleware
{
    public class ResourceMonitoringMiddleware(RequestDelegate next, ServiceMetrics metrics)
    {
        private readonly RequestDelegate _next = next;
        private readonly ServiceMetrics _metrics = metrics;

        public async Task InvokeAsync(HttpContext context)
        {
            // Identificar el servicio basado en la ruta de la petición
            var serviceName = GetServiceNameFromPath(context.Request.Path);

            // Incrementar contador de conexiones activas
            _metrics.ActiveConnections.WithLabels(serviceName).Inc();

            // Capturar estado inicial de memoria
            var memoryBefore = GC.GetTotalMemory(false);

            // Iniciar medición de tiempo
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Ejecutar el siguiente middleware en la cadena
                await _next(context);
            }
            finally
            {
                // Detener cronómetro
                stopwatch.Stop();

                // Registrar duración de la petición
                _metrics.RequestDuration
                    .WithLabels(serviceName, context.Request.Method)
                    .Observe(stopwatch.Elapsed.TotalSeconds);

                // Registrar petición completada
                _metrics.ServiceRequestCounter
                    .WithLabels(serviceName, context.Request.Method, context.Response.StatusCode.ToString())
                    .Inc();

                // Calcular y registrar memoria utilizada
                var memoryAfter = GC.GetTotalMemory(false);
                var memoryUsed = memoryAfter - memoryBefore;
                _metrics.MemoryUsage.WithLabels(serviceName).Set(memoryUsed);

                // Decrementar contador de conexiones activas
                _metrics.ActiveConnections.WithLabels(serviceName).Dec();
            }
        }

        private static string GetServiceNameFromPath(string path)
        {
            // Lógica para extraer el nombre del servicio de la ruta
            // Por ejemplo: /api/usuarios -> "usuarios"
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 2 && segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
            {
                return segments[1].ToLowerInvariant();
            }
            return "unknown";
        }
    }
}
