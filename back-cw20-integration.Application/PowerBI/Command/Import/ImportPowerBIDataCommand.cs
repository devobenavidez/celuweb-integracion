using back_cw20_integration.Application.Common.Interfaces.Mediator;

namespace back_cw20_integration.Application.PowerBI.Command.Import
{
    public class ImportPowerBIDataCommand(string queryPrefix) : IRequest<ImportPowerBIDataResponse>
    {
        public string QueryPrefix { get; set; } = queryPrefix;
    }
}
