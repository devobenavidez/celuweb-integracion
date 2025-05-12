using back_cw20_integration.Application.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;

namespace back_cw20_integration.Configuration.Authentication.Services
{
    public class AuthenticationCookieService(IHttpContextAccessor httpContextAccessor) : IAuthenticationCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public void SetAuthenticationCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthTokenIntegration", token, cookieOptions);
        }

        public void RemoveAuthenticationCookie()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AuthTokenIntegration");
        }
    }
}
