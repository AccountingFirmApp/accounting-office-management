using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries.AccountingFirmQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingFirmController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AccountingFirmController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<List<AccountingFirmDto>>> GetAll()
        {
            try
            {
                var query = new GetAllAccountingFirmQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
