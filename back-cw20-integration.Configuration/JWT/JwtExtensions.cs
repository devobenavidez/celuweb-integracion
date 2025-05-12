using back_cw20_integration.Configuration.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace back_cw20_integration.Configuration.JWT
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration) 
        {
            // Configurar autenticación
            var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);

            // Configurar environment
            var environmentSettings = new EnvironmentSettings();
            configuration.GetSection("EnvironmentSettings").Bind(environmentSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKeyJwt)),
                    ClockSkew = TimeSpan.Zero,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var token = ctx.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

                        if (string.IsNullOrEmpty(token))
                            ctx.Request.Cookies.TryGetValue("AuthTokenIntegration", out token);

                        if (!string.IsNullOrEmpty(token))
                            ctx.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
