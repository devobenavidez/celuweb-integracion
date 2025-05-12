using back_cw20_integration.Application.Common.DTO;

namespace back_cw20_integration.Application.IntegrationUser.Command.Create
{
    public class CreateIntegrationUserResponse: BasicResponseDTO
    {
        public IntegrationUserDto? User { get; set; }

        // Constructor para respuesta exitosa
        public static CreateIntegrationUserResponse SuccessResponse(IntegrationUserDto user)
        {
            return new CreateIntegrationUserResponse
            {
                Success = true,
                User = user,
                ErrorMessage = null
            };
        }

        // Constructor para respuesta fallida
        public static CreateIntegrationUserResponse FailureResponse(string errorMessage)
        {
            return new CreateIntegrationUserResponse
            {
                Success = false,
                User = null,
                ErrorMessage = errorMessage
            };
        }
    }
}
