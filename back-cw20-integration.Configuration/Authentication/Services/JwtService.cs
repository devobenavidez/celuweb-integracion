using back_cw20_integration.Application.Common.Interfaces.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace back_cw20_integration.Configuration.Authentication.Services
{
    public class JwtService(IOptions<JwtSettings> jwtSettings) : IJwtService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<string> TokenGenerateAsync(IEnumerable<Claim> claims)
        {
            return await Task.Run(() =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKeyJwt));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            });
        }

        public async Task<JsonElement> ExtractPayloadAsync(string token)
        {
            return await Task.Run(() => 
            {
                // Convertir Base64Url a Base64
                string base64 = token.Replace('-', '+').Replace('_', '/');

                // Ajustar el padding
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }

                // Decodificar Base64 a bytes y luego a string
                byte[] bytes = Convert.FromBase64String(base64);

                var encodeValue = Encoding.UTF8.GetString(bytes);

                return JsonSerializer.Deserialize<JsonElement>(encodeValue);
            });
        }
    }
}
