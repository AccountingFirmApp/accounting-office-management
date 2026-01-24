using MediatR;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Application.Queries;
using AccountingSystem.Application.DTOs;
using System.Threading.Tasks;

namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/report-types")]
    public class ReportTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// קבלת כל סוגי הדיווחים
        /// GET: api/report-types
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ReportTypeDto>>> GetAllReportTypes()  // 👈 שונה!
        {
            var query = new GetAllReportTypesQuery();
            var reportTypes = await _mediator.Send(query);
            return Ok(reportTypes);
        }

        /// <summary>
        /// קבלת סוג דיווח לפי ID
        /// GET: api/report-types/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportTypeDto>> GetReportTypeById(int id)  // 👈 שונה!
        {
            var query = new GetReportTypeByIdQuery { Id = id };
            var reportType = await _mediator.Send(query);

            if (reportType == null)
                return NotFound($"Report type with ID {id} not found");

            return Ok(reportType);
        }
    }
}