using AccountingSystem.Application.Commands.Companies;
using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.DTOs.Tasks;
using AccountingSystem.Application.Queries.Companies;
using AccountingSystem.Application.Queries.Tasks;
using AccountingSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        public async System.Threading.Tasks.Task<ActionResult<List<CompanyDto>>> GetAll([FromQuery] bool? isActive = true)
        {
            try
            {
                var query = new GetAllCompaniesQuery { IsActive = isActive };
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
        [HttpGet("worker/{id}")]
        public async System.Threading.Tasks.Task<ActionResult<CompanyDto>> GetByWorkerId(int id)
        {
            try
            {
                var query = new GetCompanyByWorkerIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        /// <summary>
        /// קבלת חברה לפי ID
        /// GET: api/companies/5
        /// </summary>
        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<CompanyDto>> GetById(int id)
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
        public async System.Threading.Tasks.Task<ActionResult<List<CompanyDto>>> GetByFirmId(int firmId)
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
        [Authorize]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateCompanyDto dto)
        {
            var firmIdClaim = User.FindFirst("FirmId")?.Value;

            if (string.IsNullOrEmpty(firmIdClaim) || !int.TryParse(firmIdClaim, out int firmId))
                return Unauthorized();

            if (User.FindFirst(ClaimTypes.Role)?.Value != "Admin")
                return Forbid();

            var command = new CreateCompanyCommand
            {
                Firmid = firmId,
                Name = dto.Name,
                Taxid = dto.TaxId,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                Notes = dto.Notes
            };

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }



        /// <summary>
        /// עדכון חברה
        /// PUT: api/companies/5
        /// </summary>
        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<CompanyDto>> Update(int id, [FromBody] Application.Commands.Companies.UpdateCompanyCommand command)
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
        /// קבלת כל המשימות של חברה מסוימת
        /// GET: api/companies/5/tasks
        /// </summary>
        [HttpGet("{id}/tasks")]
        public async System.Threading.Tasks.Task<ActionResult<List<CompanyTaskDto>>> GetCompanyTasks(int id)
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

       
        [HttpPatch("{companyId}/tasks/{taskId}/status")]
        public async Task<ActionResult> UpdateTaskStatus(int companyId, int taskId, [FromBody] UpdateTaskStatusRequest request)
        {
            // המרה מטקסט ל-Enum
            if (!Enum.TryParse<TaskStatus1>(request.Status, out var taskStatus))
            {
                return BadRequest("סטטוס לא תקין");
            }

            var command = new UpdateTaskStatusCommand
            {
                TaskId = taskId,
                Status = taskStatus 
            };

            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("task-types")]
        
        public async Task<ActionResult<List<TaskTypeDto>>> GetTaskTypes()
        {
            try
            {
                // יצירת Query חדש
                var query = new GetTaskTypesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // DTO לבקשה
        public class UpdateTaskStatusRequest
        {
            public string Status { get; set; }
        }

      

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var command = new DeleteCompanyCommand(id);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("permanent/{id}")]
        [Authorize(Roles = "SuperAdmin")] // או כל תפקיד שרק למפתחת
        public async Task<ActionResult<bool>> DeletePermanently(int id)
        {
            try
            {
                var command = new DeleteCompanyPermanentlyCommand(id);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}