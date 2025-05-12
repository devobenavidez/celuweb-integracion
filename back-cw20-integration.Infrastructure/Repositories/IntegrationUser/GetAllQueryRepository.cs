using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Application.IntegrationUser.Queries.GetAll;
using back_cw20_integration.Domain;
using System.Data;

namespace back_cw20_integration.Infrastructure.Repositories.IntegrationUser
{
    public class GetAllQueryRepository(IUnitOfWork unitOfWork) : RepositoryBase(unitOfWork), IGetAllIntegrationUserRepository
    {

        public async Task<IEnumerable<IntegrationUserModel>> GetAllIntegrationUserAsync(CancellationToken cancellationToken)
        {
            var result = await QueryAsync<IntegrationUserModel>(
                    "sp_ecosystem_cw_integration_get_all_integration_users",
                    CommandType.StoredProcedure,
                    new(),
                    cancellationToken
                );

            return result;
        }
    }
}
