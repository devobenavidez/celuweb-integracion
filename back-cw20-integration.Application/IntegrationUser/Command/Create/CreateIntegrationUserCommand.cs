using back_cw20_integration.Application.Common.Interfaces.Mediator;

namespace back_cw20_integration.Application.IntegrationUser.Command.Create
{
    public class CreateIntegrationUserCommand : IRequest<CreateIntegrationUserResponse>
    {
        public string Username { get; set; } = default!;
        public int BusinessUnit { get; set; }
    }
}
