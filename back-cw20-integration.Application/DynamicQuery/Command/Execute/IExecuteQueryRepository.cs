using System.Text.Json;

namespace back_cw20_integration.Application.DynamicQuery.Command.Execute
{
    public interface IExecuteQueryRepository
    {
        Task<List<JsonElement>> ExecuteDynamicQueryAsync(string query, List<KeyValuePair<string, string>> queryParams, string? businessUnit, int numPage, int pageSize, CancellationToken cancellationToken);
        Task<string> GetQueryByPrefixAsync(string queryPrefix, string? businessUnit, CancellationToken cancellationToken);
    }
}
