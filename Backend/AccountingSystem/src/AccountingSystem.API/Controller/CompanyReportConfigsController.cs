using MediatR;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Application.Queries;
using AccountingSystem.Application.DTOs;
using System.Threading.Tasks;
using AccountingSystem.Application.Commands;
using Microsoft.AspNetCore.Components.Forms;


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
        public async System.Threading.Tasks.Task<ActionResult<List<CompanyReportConfigDto>>> GetAllConfigs()
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
        public async System.Threading.Tasks.Task<ActionResult<List<CompanyReportConfigDto>>> GetConfigsByCompany(int companyId)
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
        public async System.Threading.Tasks.Task<ActionResult<CompanyReportConfigDto>> GetConfigById(int id)
        {
            var query = new GetConfigByIdQuery { Id = id };
            var config = await _mediator.Send(query);

            if (config == null)
                return NotFound($"Config with ID {id} not found");

            return Ok(config);
        }
        //add config

        [HttpPost]
        public async Task<ActionResult<CompanyReportConfigDto>> AddAsync(CreateCompanyReportConfigDto request)
        {
            try
            {
                var command = new CreateCompanyConfigReportCommand
                {
                    CompanyId = request.CompanyId,
                    ReportTypeId = request.ReportTypeId,
                    FrequencyId = request.FrequencyId,
                    DayOfMonth = request.DayOfMonth,
                    Year = request.Year
                };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CompanyReportConfigDto>> UpdateAsync(
            int id,
            UpdateCompanyReportConfigDto request)
        {
            try
            {
                var command = new UpdateCompanyConfigReportCommand
                {
                    Id = id,
                    FrequencyId = request.FrequencyId,
                    DayOfMonth = request.DayOfMonth,
                    IsActive=request.IsActive
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
            }
   }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyReportConfigDto>> DeleteAsync(int id  )
        {
            try
            {
                var command = new DeleteCompanyConfigReportCommand
                {
                    Id = id
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}