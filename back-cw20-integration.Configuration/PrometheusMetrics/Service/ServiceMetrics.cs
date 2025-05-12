using Prometheus;

namespace back_cw20_integration.Configuration.PrometheusMetrics.Service
{
    public class ServiceMetrics
    {
        // Contadores para peticiones por servicio
        public readonly Counter ServiceRequestCounter;

        // Histogramas para tiempos de respuesta
        public readonly Histogram RequestDuration;

        // Gauge para memoria utilizada
        public readonly Gauge MemoryUsage;

        // Gauge para conexiones activas
        public readonly Gauge ActiveConnections;

        public ServiceMetrics()
        {
            // Inicializar métricas con nombres y descripciones apropiadas
            ServiceRequestCounter = Metrics.CreateCounter(
                "api_service_requests_total",
                "Número total de peticiones por servicio",
                new CounterConfiguration
                {
                    LabelNames = ["service", "method", "status_code"]
                }
            );

            RequestDuration = Metrics.CreateHistogram(
                "api_request_duration_seconds",
                "Duración de las peticiones en segundos",
                new HistogramConfiguration
                {
                    LabelNames = ["service", "method"],
                    Buckets = [0.01, 0.05, 0.1, 0.5, 1, 2, 5, 10]
                }
            );

            MemoryUsage = Metrics.CreateGauge(
                "api_memory_usage_bytes",
                "Consumo de memoria en bytes por servicio",
                new GaugeConfiguration
                {
                    LabelNames = ["service"]
                }
            );

            ActiveConnections = Metrics.CreateGauge(
                "api_active_connections",
                "Número de conexiones activas",
                new GaugeConfiguration
                {
                    LabelNames = ["service"]
                }
            );
        }
    }
}
