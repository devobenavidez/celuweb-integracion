using back_cw20_integration.Application.Common.Interfaces.Authentication;
using back_cw20_integration.Application.Common.Interfaces.JWT;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Domain;

namespace back_cw20_integration.Application.Authentication.Command.Login
{
    public class LoginCommandHandler(ILoginCommandRepository repository
        , IJwtService jwtService
        , IAuthenticationCookieService cookieService
        , IClaimsSettings claimsSettings)
        : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly ILoginCommandRepository _repository = repository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IAuthenticationCookieService _cookieService = cookieService;
        private readonly IClaimsSettings _claimsSettings = claimsSettings;
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _cookieService.RemoveAuthenticationCookie();
            IntegrationUserModel? user = await _repository.FindUserByCredentialsAsync(request.Username, request.AccessToken, cancellationToken);

            if (user != null)
            {
                
                var jsonPayload = await _jwtService.ExtractPayloadAsync(request.AccessToken);

                var userIdAccessToken = jsonPayload.GetProperty("sub").ToString();

                if(!user.UserId.Equals(userIdAccessToken, StringComparison.CurrentCultureIgnoreCase))
                    return LoginResponse.FailureResponse("Credenciales Invalidas");

                string token = await _jwtService.TokenGenerateAsync(_claimsSettings.ClaimsToNormalToken(user));

                _cookieService.SetAuthenticationCookie(token);

                return LoginResponse.SuccessResponse(token, DateTime.UtcNow.AddHours(12));
            }

            return LoginResponse.FailureResponse("Credenciales Invalidas");   
        }
    }
}
