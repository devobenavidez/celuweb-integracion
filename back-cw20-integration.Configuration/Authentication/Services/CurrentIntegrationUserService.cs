using back_cw20_integration.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace back_cw20_integration.Configuration.Authentication.Services
{
    public class CurrentIntegrationUserService(IHttpContextAccessor httpContextAccessor) : ICurrentIntegrationUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue("username");
        public string? BusinessUnit => _httpContextAccessor.HttpContext?.User?.FindFirstValue("business_unit");
        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    }
}
