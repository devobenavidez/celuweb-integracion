using back_cw20_integration.Domain;

namespace back_cw20_integration.Application.Authentication.Command.Login
{
    public interface ILoginCommandRepository
    {
        Task<IntegrationUserModel?> FindUserByCredentialsAsync(string username, string accessToken, CancellationToken cancellationToken);
    }
}
