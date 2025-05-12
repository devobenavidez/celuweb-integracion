using back_cw20_integration.Configuration.Authentication.Interfaces;

namespace back_cw20_integration.Configuration.Authentication
{
    public class JwtSettings : IJwtSettings
    {
        public string SecretKeyJwt { get; set; } = default!;
    }
}
