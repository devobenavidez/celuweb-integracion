using back_cw20_integration.Application.Common.DTO;

namespace back_cw20_integration.Application.Authentication.Command.Login
{
    public class LoginResponse : BasicResponseDTO
    {
        public string? Token { get; set; } = default!;
        public DateTime? ExpiresAt { get; set; }

        // Constructor para respuesta exitosa
        public static LoginResponse SuccessResponse(string token, DateTime expiryTime)
        {
            return new LoginResponse
            {
                Success = true,
                Token = token,
                ExpiresAt = expiryTime,
                ErrorMessage = null
            };
        }

        // Constructor para respuesta fallida
        public static LoginResponse FailureResponse(string errorMessage)
        {
            return new LoginResponse
            {
                Success = false,
                Token = null,
                ExpiresAt = null,
                ErrorMessage = errorMessage
            };
        }
    }
}
