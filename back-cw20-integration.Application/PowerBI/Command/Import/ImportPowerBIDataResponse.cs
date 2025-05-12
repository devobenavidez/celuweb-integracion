using back_cw20_integration.Application.Common.DTO;

namespace back_cw20_integration.Application.PowerBI.Command.Import
{
    public class ImportPowerBIDataResponse : BasicResponseDTO
    {
        public string QueryPrefix { get; set; } = default!;
        public int QuantityResults { get; set; }

        public static ImportPowerBIDataResponse SuccessResponse(string queryPrefix, int quantityResults) 
        {
            return new ImportPowerBIDataResponse
            {
                Success = true,
                QueryPrefix = queryPrefix,
                QuantityResults = quantityResults,
                ErrorMessage = string.Empty
            };
        }

        public static ImportPowerBIDataResponse FailureResponse(string errorMessage)
        {
            return new ImportPowerBIDataResponse
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
