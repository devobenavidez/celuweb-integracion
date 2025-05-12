using back_cw20_integration.Domain;

namespace back_cw20_integration.Application.IntegrationUser.Queries.GetAll
{
    public interface IGetAllIntegrationUserRepository
    {
        Task<IEnumerable<IntegrationUserModel>> GetAllIntegrationUserAsync(CancellationToken cancellationToken);
    }
}
