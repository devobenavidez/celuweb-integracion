using back_cw20_integration.Application.Common.Interfaces.Mediator;

namespace back_cw20_integration.Application.Authentication.Command.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
    }
}
