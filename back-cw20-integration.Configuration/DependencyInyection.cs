using back_cw20_integration.Application.Common.Interfaces;
using back_cw20_integration.Application.Common.Interfaces.Authentication;
using back_cw20_integration.Application.Common.Interfaces.JWT;
using back_cw20_integration.Configuration.Authentication;
using back_cw20_integration.Configuration.Authentication.Services;
using back_cw20_integration.Configuration.Authentication.Settings;
using back_cw20_integration.Configuration.JWT;
using back_cw20_integration.Configuration.PrometheusMetrics;
using back_cw20_integration.Configuration.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace back_cw20_integration.Configuration
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddAllConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra el HttpContextAccessor para acceder al HttpContext
            services.AddHttpContextAccessor();
            // Configurar JWT
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IClaimsSettings, ClaimsSettings>();

            services.AddSwaggerDocumentation(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddPrometheusMetrics();

            // Registra el servicio de cookies
            services.AddScoped<IAuthenticationCookieService, AuthenticationCookieService>();
            services.AddScoped<ICurrentIntegrationUserService, CurrentIntegrationUserService>();

            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
