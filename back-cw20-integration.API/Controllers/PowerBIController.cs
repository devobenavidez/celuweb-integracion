using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.PowerBI.Command.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_cw20_integration.API.Controllers
{
    [ApiController]
    [Route("api/powerbi")]
    [Authorize]
    public class PowerBIController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("import_data/{queryPrefix}")]
        [Authorize(Roles = "INTEGRATION")]
        public async Task<IActionResult> ImportData([FromRoute] string queryPrefix, CancellationToken cancellationToken)
        {
            ImportPowerBIDataCommand command = new(queryPrefix);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
