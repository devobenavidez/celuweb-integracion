using back_cw20_integration.Application.Common.Interfaces.JWT;
using back_cw20_integration.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace back_cw20_integration.Configuration.Authentication.Settings
{
    public class ClaimsSettings : IClaimsSettings
    {
        public List<Claim> ClaimsToBasicToken(IntegrationUserModel user)
        {
            return
            [
                new("username", user.Username),
                new("business_unit", user.BusinessUnit.ToString()),
                new("sub", user.UserId),
                new(ClaimTypes.Role, user.Role)
            ];
        }

        public List<Claim> ClaimsToNormalToken(IntegrationUserModel user)
        {
            return
                [
                    new("username", user.Username),
                    new("business_unit", user.BusinessUnit.ToString()),
                    new("sub", user.UserId),
                    new(ClaimTypes.Role, user.Role),
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(12).ToUnixTimeSeconds().ToString()),
                ];
        }
    }
}
