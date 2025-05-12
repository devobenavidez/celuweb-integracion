using back_cw20_integration.Domain;
using System.Security.Claims;

namespace back_cw20_integration.Application.Common.Interfaces.JWT
{
    public interface IClaimsSettings
    {
        List<Claim> ClaimsToBasicToken(IntegrationUserModel user);
        List<Claim> ClaimsToNormalToken(IntegrationUserModel user);
    }
}
