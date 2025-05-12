using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Application.PowerBI.Command.Import
{
    public interface IImportPowerBIDataCommandRepository
    {
        Task<PowerBIAdomdConfiguration?> GetPowerBIAdomdConfigurationAsync(string connectionPrefix, string businessUnit, CancellationToken cancellationToken);
        Task<PowerBIDAXConfiguration?> GetPowerBIDAXConfigurationAsync(string queryPrefix, string businessUnit, CancellationToken cancellationToken);
        Task CleanTemporalTableAsync(string tempTableName, string businessUnit, CancellationToken cancellationToken);
        Task BulkInsertAsync(string tempTableName, DataTable data, CancellationToken cancellationToken);
    }
}
