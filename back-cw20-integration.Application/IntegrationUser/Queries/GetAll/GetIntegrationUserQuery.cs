using back_cw20_integration.Application.Common.Interfaces.Mediator;

namespace back_cw20_integration.Application.IntegrationUser.Queries.GetAll
{
    public class GetIntegrationUserQuery(bool refreshQuery) : IRequest<GetIntegrationUserResponse>
    {
        public bool RefreshCache { get; set; } = refreshQuery;
    }
}
