using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Application.IntegrationUser.Command.Create;
using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Infrastructure.Repositories.IntegrationUser
{
    public class CreateCommandRepository(IUnitOfWork unitOfWork) 
        : RepositoryBase(unitOfWork), ICreateIntegrationUserRepository
    {
        public async Task<bool> CreateAsync(IntegrationUserModel user, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                user_id = user.UserId,
                username = user.Username,
                access_token = user.AccessToken,
                business_unit = user.BusinessUnit,
                role = user.Role,
            };

            return await ExecuteAsync(
                       "sp_ecosystem_cw_integration_create_user",
                        CommandType.StoredProcedure,
                        parameters,
                        cancellationToken
                    ) > 0;
        }
        public async Task<bool> FindUsernameAndBusinessUnitAsync(string username, int businessUnit, CancellationToken cancellationToken)
        {
            var parameters = new { username, business_unit = businessUnit };

            string? exist = await QueryFirstOrDefaultAsync<string>(
                                    "sp_ecosystem_cw_integration_find_user",
                                    CommandType.StoredProcedure,
                                    parameters,
                                    cancellationToken
                                  );

            return !string.IsNullOrEmpty(exist);
        }
    }
}
