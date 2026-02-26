using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.Queries.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccountingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskConfigurationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("matrix")]
        public async Task<IActionResult> GetMatrix()
        {
            var result = await _mediator.Send(new GetTaskMatrixQuery());
            return Ok(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveConfiguration([FromBody] SaveTaskConfigurationCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}