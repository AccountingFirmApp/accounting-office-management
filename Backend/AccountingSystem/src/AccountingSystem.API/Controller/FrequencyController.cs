using AccountingSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FrequencyController : ControllerBase
    {
        //[HttpGet("{id}")]
        //public async Task<ActionResult<FrequencyDto>> GetById(int id)
        //{
        //    var query = new GetFrequencyByIdQuery { Id = id };
        //    var result = await _mediator.Send(query);

        //    if (result == null)
        //        return NotFound();

        //    return Ok(result);
        //}
    }
}
