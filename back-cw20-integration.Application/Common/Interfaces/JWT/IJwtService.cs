using System.Security.Claims;
using System.Text.Json;

namespace back_cw20_integration.Application.Common.Interfaces.JWT
{
    public interface IJwtService
    {
        Task<string> TokenGenerateAsync(IEnumerable<Claim> claims);
        Task<JsonElement> ExtractPayloadAsync(string token);
    }
}
