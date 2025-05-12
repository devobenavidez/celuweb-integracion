using back_cw20_integration.API.Extensions;
using back_cw20_integration.API.Middleware;
using back_cw20_integration.Application;
using back_cw20_integration.Configuration;
using back_cw20_integration.Configuration.PrometheusMetrics;
using back_cw20_integration.Configuration.Swagger;
using back_cw20_integration.Infrastructure;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Usar el método de extensión para configurar Swagger

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddAllConfigurations(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddRateLimiterExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwaggerDocumentation();

app.UsePrometheusMetrics();
app.MapMetrics();
app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();