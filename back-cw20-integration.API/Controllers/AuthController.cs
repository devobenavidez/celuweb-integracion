using back_cw20_integration.Application.Authentication.Command.Login;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace back_cw20_integration.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.Success) 
                return Ok(result);
            
            return Unauthorized(result);
        }

    }
}
