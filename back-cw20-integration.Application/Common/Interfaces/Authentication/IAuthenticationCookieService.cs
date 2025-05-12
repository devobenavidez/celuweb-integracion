using Microsoft.AspNetCore.Http;

namespace back_cw20_integration.Application.Common.Interfaces.Authentication
{
    public interface IAuthenticationCookieService
    {
        void SetAuthenticationCookie(string token);
        void RemoveAuthenticationCookie();
    }
}
