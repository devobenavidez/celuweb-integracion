using back_cw20_integration.Application.Common.DTO;
using System.Text.Json;

namespace back_cw20_integration.Application.DynamicQuery.Command.Execute
{
    public class ExecuteQueryResponse : BasicResponseDTO
    {
        public string QueryPrefix { get; set; } = default!;
        public List<KeyValuePair<string, string>> Filters { get; set; } = [];
        public int NumPage { get; set; } = default!;
        public int PageSize { get; set; } = default!;
        public int QuantityResults { get; set; }
        public List<JsonElement> Results { get; set; } = [];

        // Constructor para respuesta exitosa
        public static ExecuteQueryResponse SuccessResponse(ExecuteQueryCommand command, int quantityResults, List<JsonElement> Results)
        {
            return new ExecuteQueryResponse
            {
                Success = true,
                ErrorMessage = null,
                QueryPrefix = command.QueryPrefix,
                Filters = command.QueryParams,
                NumPage = command.NumPage,
                PageSize = command.PageSize,
                QuantityResults = quantityResults,
                Results = Results
            };
        }

        // Constructor para respuesta fallida
        public static ExecuteQueryResponse FailureResponse(string errorMessage)
        {
            return new ExecuteQueryResponse
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
