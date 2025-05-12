using back_cw20_integration.Application.Common.DTO;

namespace back_cw20_integration.Application.IntegrationUser.Queries.GetAll
{
    public class GetIntegrationUserResponse: BasicResponseDTO
    {
        public List<IntegrationUserDto> Users { get; set; } = [];

        // Constructor para respuesta exitosa
        public static GetIntegrationUserResponse SuccessResponse(List<IntegrationUserDto> users)
        {
            return new GetIntegrationUserResponse
            {
                Success = true,
                Users = users,
                ErrorMessage = null
            };
        }

        // Constructor para respuesta fallida
        public static GetIntegrationUserResponse FailureResponse(string errorMessage)
        {
            return new GetIntegrationUserResponse
            {
                Success = false,
                Users = [],
                ErrorMessage = errorMessage
            };
        }
    }
}
