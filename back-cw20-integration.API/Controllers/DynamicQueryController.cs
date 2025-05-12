using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.DynamicQuery.Command.Execute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace back_cw20_integration.API.Controllers
{
    [ApiController]
    [Route("api/dynamic_queries")]
    [Authorize]

    public class DynamicQueryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("execute_query")]
        [Authorize(Roles = "INTEGRATION")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> ExecuteQuery([FromBody] ExecuteQueryCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
