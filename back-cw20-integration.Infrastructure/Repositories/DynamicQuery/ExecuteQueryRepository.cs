using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Application.DynamicQuery.Command.Execute;
using Dapper;
using System.Data;
using System.Text.Json;

namespace back_cw20_integration.Infrastructure.Repositories.DynamicQuery
{
    public class ExecuteQueryRepository(IUnitOfWork unitOfWork) 
        : RepositoryBase(unitOfWork), IExecuteQueryRepository
    {
        public async Task<List<JsonElement>> ExecuteDynamicQueryAsync(string query, List<KeyValuePair<string, string>> queryParams, string? businessUnit, int numPage, int pageSize, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            foreach (KeyValuePair<string, string> param in queryParams)
                parameters.Add(param.Key, param.Value);

            parameters.Add("business_unit", businessUnit);
            parameters.Add("numPage", numPage);
            parameters.Add("pageSize", pageSize);

            string jsonResult = await QueryFirstOrDefaultAsync<string>(
                        query,
                        CommandType.Text,
                        parameters: parameters,
                        cancellationToken: cancellationToken
                    ) ?? string.Empty;

            return string.IsNullOrEmpty(jsonResult)
                ? []
                : JsonSerializer.Deserialize<List<JsonElement>>(jsonResult) ?? [];
        }

        public async Task<string> GetQueryByPrefixAsync(string queryPrefix, string? businessUnit, CancellationToken cancellationToken)
        {
            var parameters = new { query_prefix = queryPrefix, business_unit = businessUnit };

            return await QueryFirstOrDefaultAsync<string>(
                "sp_get_sql_query_to_execute",
                CommandType.StoredProcedure,
                parameters,
                cancellationToken
                ) ?? string.Empty;
        }
    }
}
