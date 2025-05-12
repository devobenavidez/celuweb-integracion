using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Application.PowerBI.Command.Import;
using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Infrastructure.Repositories.PowerBI
{
    public class ImportPowerBIDataCommandRepository(IUnitOfWork unitOfWork) : RepositoryBase(unitOfWork), IImportPowerBIDataCommandRepository
    {
        public async Task BulkInsertAsync(string tempTableName, DataTable data, CancellationToken cancellationToken)
        {
            await BulkInsertAsync(tempTableName, data, cancellationToken);
        }

        public async Task CleanTemporalTableAsync(string tempTableName, string businessUnit, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                temp_table_name = tempTableName,
                business_unit = businessUnit,
            };

            await ExecuteAsync(
                "sp_ecosystem_cw_integration_clean_powerbi_temp_table",
                CommandType.StoredProcedure,
                parameters,
                cancellationToken
            );
        }

        public async Task<PowerBIAdomdConfiguration?> GetPowerBIAdomdConfigurationAsync(string connectionPrefix, string businessUnit, CancellationToken cancellationToken)
        {
            var parameters = new { connection_prefix = connectionPrefix, business_unit = businessUnit };

            return await QueryFirstOrDefaultAsync<PowerBIAdomdConfiguration>(
                "sp_get_powerbi_adomd_configuration",
                CommandType.StoredProcedure,
                parameters,
                cancellationToken
                ) ?? null;
        }

        public async Task<PowerBIDAXConfiguration?> GetPowerBIDAXConfigurationAsync(string queryPrefix, string businessUnit, CancellationToken cancellationToken)
        {
            var parameters = new { query_prefix = queryPrefix, business_unit = businessUnit };

            return await QueryFirstOrDefaultAsync<PowerBIDAXConfiguration>(
                "sp_get_powerbi_dax_configuration",
                CommandType.StoredProcedure,
                parameters,
                cancellationToken
                ) ?? null;
        }
    }
}
