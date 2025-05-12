using back_cw20_integration.Application.Authentication.Command.Login;

namespace back_cw20_integration.Application.Common.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> AuthenticateAsync(string username, string accessToken);
    }
}