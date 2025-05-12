using back_cw20_integration.Application.Common.Interfaces;
using back_cw20_integration.Application.Common.Interfaces.Cache;
using back_cw20_integration.Application.Common.Interfaces.Mediator;

namespace back_cw20_integration.Application.DynamicQuery.Command.Execute
{
    public class ExecuteQueryCommandHandler(ICurrentIntegrationUserService currentIntegrationUserService
        , IExecuteQueryRepository repository
        , ICacheService cacheService) : 
        IRequestHandler<ExecuteQueryCommand, ExecuteQueryResponse>
    {
        private readonly ICurrentIntegrationUserService _currentIntegrationUserService = currentIntegrationUserService;
        private readonly IExecuteQueryRepository _repository = repository;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<ExecuteQueryResponse> Handle(ExecuteQueryCommand request, CancellationToken cancellationToken)
        {
            var valueParams = request.QueryParams
                                .Select(par => $"_{par.Key}_{par.Value}")
                                .ToList();

            string cacheKey = $"dynamic_query_{request.QueryPrefix}_{string.Join("", valueParams)}_{request.NumPage}_{request.PageSize}";

            var cachedQuery = await _cacheService.GetByRefreshCacheAsync<ExecuteQueryResponse>(cacheKey, request.RefreshCache);

            if (cachedQuery != null)
                return cachedQuery;

            var businessUnit = _currentIntegrationUserService.BusinessUnit;

            var query = await _repository.GetQueryByPrefixAsync(request.QueryPrefix, businessUnit, cancellationToken);

            if (string.IsNullOrEmpty(query)) return ExecuteQueryResponse.FailureResponse($"No se encontro configuración del prefijo {request.QueryPrefix}");

            var results = await _repository.ExecuteDynamicQueryAsync(query, request.QueryParams, businessUnit, request.NumPage, request.PageSize, cancellationToken);

            // Guardar en caché
            await _cacheService.SetAsync(cacheKey, ExecuteQueryResponse.SuccessResponse(request, results.Count, results), TimeSpan.FromMinutes(30));

            return ExecuteQueryResponse.SuccessResponse(request, results.Count, results);
        }
    }
}
