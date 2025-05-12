using back_cw20_integration.Application.Common.Interfaces;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Application.PowerBI.Interfaces;
using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Application.PowerBI.Command.Import
{
    public class ImportPowerBIDataCommandHandler(ICurrentIntegrationUserService currentIntegrationUserService
        , IImportPowerBIDataCommandRepository repository
        , IPowerBIService service
        , IUnitOfWork unitOfWork) :
        IRequestHandler<ImportPowerBIDataCommand, ImportPowerBIDataResponse>
    {
        private readonly ICurrentIntegrationUserService _currentIntegrationUserService = currentIntegrationUserService;
        private readonly IImportPowerBIDataCommandRepository _repository = repository;
        private readonly IPowerBIService _powerBIService = service;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ImportPowerBIDataResponse> Handle(ImportPowerBIDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                string businessUnit = _currentIntegrationUserService.BusinessUnit ?? string.Empty;
                PowerBIDAXConfiguration? queryConfiguration = await _repository.GetPowerBIDAXConfigurationAsync(request.QueryPrefix, businessUnit, cancellationToken);

                if (queryConfiguration == null)
                    return ImportPowerBIDataResponse.FailureResponse($"No se encontró configuración para el prefijo {request.QueryPrefix}");

                PowerBIAdomdConfiguration? configuration = await _repository.GetPowerBIAdomdConfigurationAsync(queryConfiguration.ConnectionPrefix, businessUnit, cancellationToken);

                if (configuration == null)
                    return ImportPowerBIDataResponse.FailureResponse($"No se encontró configuración de conexión para el prefijo {queryConfiguration.ConnectionPrefix}");

                DataTable data = await _powerBIService.ExecuteQueryAdomdAsync(configuration, queryConfiguration, cancellationToken);

                if (data == null)
                    return ImportPowerBIDataResponse.FailureResponse($"No se encontrarón datos en el cliente para el prefijo {request.QueryPrefix}");

                data = await _powerBIService.AddParamBusinessUnitAsync(data, businessUnit, cancellationToken);

                string tempTableName = queryConfiguration.TempTableName;

                await _repository.CleanTemporalTableAsync(tempTableName, businessUnit, cancellationToken);

                await _repository.BulkInsertAsync(tempTableName, data, cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return ImportPowerBIDataResponse.SuccessResponse(request.QueryPrefix, data.Rows.Count);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ImportPowerBIDataResponse.FailureResponse("Ocurrio un error al tratar de importar los datos de la query");
            }
        }
    }
}
