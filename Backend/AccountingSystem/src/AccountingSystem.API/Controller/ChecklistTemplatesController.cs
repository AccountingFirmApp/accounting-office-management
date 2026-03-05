using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Application.Queries.Tasks;
using AccountingSystem.Application.Commands.Tasks;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChecklistTemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChecklistTemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("template/{taskTypeId}")]
        public async Task<ActionResult<ChecklistTemplateDto>> GetTemplate(int taskTypeId)
        {
            var result = await _mediator.Send(new GetChecklistTemplateByTaskTypeQuery(taskTypeId));
            return Ok(result);
        }

        [HttpPost("template")]
        public async Task<IActionResult> SaveTemplate([FromBody] SaveChecklistTemplateCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "התבנית נשמרה בהצלחה" });
        }
    }
}