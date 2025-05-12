using back_cw20_integration.Application.Common.DTO;
using back_cw20_integration.Application.Common.Interfaces.Cache;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Domain;

namespace back_cw20_integration.Application.IntegrationUser.Queries.GetAll
{
    public class GetIntegrationUserQueryHandler(IGetAllIntegrationUserRepository repository
        , ICacheService cacheService) :
        IRequestHandler<GetIntegrationUserQuery, GetIntegrationUserResponse>
    {
        private readonly IGetAllIntegrationUserRepository _repository = repository;
        private readonly ICacheService _cacheService = cacheService;
        public async Task<GetIntegrationUserResponse> Handle(GetIntegrationUserQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "integration_users_list_all";

            var cachedQuery = await _cacheService.GetByRefreshCacheAsync<GetIntegrationUserResponse>(cacheKey, request.RefreshCache);

            if (cachedQuery != null) 
                return cachedQuery;

            IEnumerable<IntegrationUserModel> users = await _repository.GetAllIntegrationUserAsync(cancellationToken);

            if (users.Any()) 
            {
                List<IntegrationUserDto> usersDto = [.. users.Select(user => new IntegrationUserDto(user.Username, user.AccessToken, user.BusinessUnit))];

                await _cacheService.SetAsync(cacheKey, GetIntegrationUserResponse.SuccessResponse(usersDto), TimeSpan.FromMinutes(30));

                return GetIntegrationUserResponse.SuccessResponse(usersDto);
            }

            return GetIntegrationUserResponse.FailureResponse("No se encontrarón usuarios");
        }
    }
}
