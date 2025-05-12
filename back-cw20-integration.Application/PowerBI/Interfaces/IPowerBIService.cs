using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Application.PowerBI.Interfaces
{
    public interface IPowerBIService
    {
        Task<DataTable> AddParamBusinessUnitAsync(DataTable data, string businessUnit, CancellationToken cancellationToken);
        Task<DataTable> ExecuteQueryAdomdAsync(PowerBIAdomdConfiguration adomdConfiguration, PowerBIDAXConfiguration powerBIDAXConfiguration, CancellationToken cancellationToken);
    }
}
