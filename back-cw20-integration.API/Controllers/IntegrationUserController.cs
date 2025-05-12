using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.IntegrationUser.Command.Create;
using back_cw20_integration.Application.IntegrationUser.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace back_cw20_integration.API.Controllers
{
    [ApiController]
    [Route("api/integration_user")]
    [Authorize(Roles = "ADMIN")]
    public class IntegrationUserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create")]
        public async Task<IActionResult> CreateIntegrationUser([FromBody] CreateIntegrationUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("users")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> IntegrationUsersGetAll(CancellationToken cancellationToken, [FromQuery] bool refreshCache = false)
        {
            GetIntegrationUserQuery query = new(refreshCache);

            var result = await _mediator.Send(query, cancellationToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
