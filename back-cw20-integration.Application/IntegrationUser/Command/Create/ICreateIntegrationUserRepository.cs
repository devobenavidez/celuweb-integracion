using back_cw20_integration.Domain;

namespace back_cw20_integration.Application.IntegrationUser.Command.Create
{
    public interface ICreateIntegrationUserRepository
    {
        Task<bool> CreateAsync(IntegrationUserModel user, CancellationToken cancellationToken);
        Task<bool> FindUsernameAndBusinessUnitAsync(string username, int businessUnit, CancellationToken cancellationToken);
    }
}
