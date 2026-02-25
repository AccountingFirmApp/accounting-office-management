 using AccountingSystem.Application.Commands.Workers;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Application.Queries.Workers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// קבלת כל העובדים
        /// GET: api/workers
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<List<WorkerDto>>> GetAll([FromQuery] bool? isActive = true)
        {
            try
            {
                var query = new GetAllWorkersQuery { IsActive = isActive };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// קבלת עובד לפי ID
        /// GET: api/workers/5
        /// </summary>
        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<WorkerDto>> GetById(int id)
        {
            try
            {
                var query = new GetWorkerByIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// קבלת עובדים לפי משרד
        /// GET: api/workers/firm/1
        /// </summary>
        [HttpGet("firm/{firmId}")]
        public async System.Threading.Tasks.Task<ActionResult<List<WorkerDto>>> GetByFirmId(int firmId)
        {
            try
            {
                var query = new GetWorkersByFirmIdQuery(firmId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// יצירת עובד חדש
        /// POST: api/workers
        /// </summary>
        [HttpPost]
        [Authorize] // ✅ חובה!
        public async Task<ActionResult<WorkerDto>> Create([FromBody] CreateWorkerCommand command)
        {
            try
            {
                // 🔥 שלב 1: קבלת FirmId מה-Token
                var firmIdClaim = User.FindFirst("FirmId")?.Value;

                if (string.IsNullOrEmpty(firmIdClaim) || !int.TryParse(firmIdClaim, out int firmId))
                {
                    return Unauthorized(new { message = "לא נמצא FirmId עבור המשתמש המחובר" });
                }

                // 🔥 שלב 2: בדיקה שהמשתמש הוא מנהל
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
                if (roleClaim != "Admin") // שנה ל-"Admin" או מה שמתאים לך
                {
                    return Forbid();
                }

                // ✅ שלב 3: הזרקת FirmId לתוך ה-Command
                command.Firmid = firmId;

                // ✅ שלב 4: שליחה ל-Handler
                var result = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// עדכון עובד
        /// PUT: api/workers/5
        /// </summary>
        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<WorkerDto>> Update(int id, [FromBody] Application.Commands.Workers.UpdateWorkerCommand command)
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
        /// מחיקת עובד
        /// DELETE: api/workers/5
        /// </summary>
        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            try
            {
                var command = new Application.Commands.Workers.DeleteWorkerCommand(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        /// <summary>
        /// קבלת כל החברות של עובדת
        /// GET: api/workers/5/companies
        /// </summary>
        [HttpGet("{workerId}/companies")]
        public async System.Threading.Tasks.Task<ActionResult<List<CompanyDto>>> GetWorkerCompanies(int workerId)
        {
            try
            {
                var query = new GetWorkerCompaniesQuery(workerId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}