using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Application.Commands;
using Microsoft.AspNetCore.Authorization;
namespace AccountingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportInstanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportInstanceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<List<ReportInstanceDto>>> CreateAsync(DateTime date)
        {
            var command = new GenerateAutoReportInstanceCommand(date);
            var res = await _mediator.Send(command);
            return Ok(res);
        }
    }
}
