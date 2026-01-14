using AccountingSystem.Application.Commands.Companies;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// קבלת כל החברות
        /// GET: api/companies
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> GetAll()
        {
            try
            {
                var query = new GetAllCompaniesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// קבלת חברה לפי ID
        /// GET: api/companies/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(int id)
        {
            try
            {
                var query = new GetCompanyByIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// קבלת חברות לפי משרד
        /// GET: api/companies/firm/1
        /// </summary>
        [HttpGet("firm/{firmId}")]
        public async Task<ActionResult<List<CompanyDto>>> GetByFirmId(int firmId)
        {
            try
            {
                var query = new GetCompaniesByFirmIdQuery(firmId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// יצירת חברה חדשה
        /// POST: api/companies
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] Application.Commands.Companies.CreateCompanyCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// עדכון חברה
        /// PUT: api/companies/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDto>> Update(int id, [FromBody] Application.Commands.Companies.UpdateCompanyCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest(new { message = "ID לא תואם" });
                }

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// מחיקת חברה
        /// DELETE: api/companies/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new Application.Commands.Companies.DeleteCompanyCommand(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// קבלת כל המשימות של חברה מסוימת
        /// GET: api/companies/5/tasks
        /// </summary>
        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<List<TaskDto>>> GetCompanyTasks(int id)
        {
            try
            {
                var query = new AccountingSystem.Application.Queries.Tasks.GetTasksByCompanyIdQuery(id);
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