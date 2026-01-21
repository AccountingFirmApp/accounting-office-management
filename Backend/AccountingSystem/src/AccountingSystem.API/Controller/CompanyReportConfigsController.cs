using MediatR;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Application.Queries;
using AccountingSystem.Application.DTOs;

namespace AccountingSystem.API.Controllers
{
    [ApiController]
    [Route("api/company-report-configs")]
    public class CompanyReportConfigsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyReportConfigsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// קבלת כל הקונפיגורציות (שילובי חברה + סוג דיווח)
        /// GET: api/company-report-configs
        /// </summary>
        [HttpGet]
        public async AccountingSystem.Domain.Entities.Task<ActionResult<List<CompanyReportConfigDto>>> GetAllConfigs()
        {
            var query = new GetAllConfigsQuery();
            var configs = await _mediator.Send(query);
            return Ok(configs);
        }

        /// <summary>
        /// קבלת קונפיגורציות לפי חברה
        /// GET: api/company-report-configs/company/5
        /// </summary>
        [HttpGet("company/{companyId}")]
        public async AccountingSystem.Domain.Entities.Task<ActionResult<List<CompanyReportConfigDto>>> GetConfigsByCompany(int companyId)
        {
            var query = new GetConfigsByCompanyIdQuery { CompanyId = companyId };
            var configs = await _mediator.Send(query);
            return Ok(configs);
        }

        /// <summary>
        /// קבלת קונפיגורציה לפי ID
        /// GET: api/company-report-configs/5
        /// </summary>
        [HttpGet("{id}")]
        public async AccountingSystem.Domain.Entities.Task<ActionResult<CompanyReportConfigDto>> GetConfigById(int id)
        {
            var query = new GetConfigByIdQuery { Id = id };
            var config = await _mediator.Send(query);

            if (config == null)
                return NotFound($"Config with ID {id} not found");

            return Ok(config);
        }
    }
}