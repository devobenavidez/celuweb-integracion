using back_cw20_integration.Application.Authentication.Command.Login;
using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Infrastructure.Repositories.Authentication
{
    public class LoginCommandRepository(IUnitOfWork unitOfWork) : RepositoryBase(unitOfWork), ILoginCommandRepository
    {
        public async Task<IntegrationUserModel?> FindUserByCredentialsAsync(string username, string accessToken, CancellationToken cancellationToken)
        {
            var parameters = new { username, access_token = accessToken };

            return await QueryFirstOrDefaultAsync<IntegrationUserModel>(
                "sp_ecosystem_cw_integration_login",
                CommandType.StoredProcedure,
                parameters,
                cancellationToken);
        }
    }
}
