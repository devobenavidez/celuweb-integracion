using System.Threading.RateLimiting;

namespace back_cw20_integration.API.Extensions
{
    public static class RateLimitExtension
    {
        public static IServiceCollection AddRateLimiterExtension(this IServiceCollection services) 
        {
            services.AddRateLimiter(static options =>
            {
                options.AddPolicy("fixed", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(10)
                        }
                    )
                );

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfterSeconds)
                        ? retryAfterSeconds
                        : TimeSpan.FromSeconds(10);

                    context.HttpContext.Response.Headers.RetryAfter = retryAfter.ToString();

                    var response = new
                    {
                        Error = "Has excedido el límite de solicitudes permitidas",
                        Message = "Por favor, intenta nuevamente más tarde",
                        RetryAfter = $"{retryAfter} segundos",
                        Timestamp = DateTime.UtcNow
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                };
            });

            return services;
        }
    }
}
