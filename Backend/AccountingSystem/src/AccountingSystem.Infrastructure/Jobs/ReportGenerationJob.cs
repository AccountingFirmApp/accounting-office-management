using AccountingSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Infrastructure.Jobs
{
    public class ReportGenerationJob
    {
        private readonly IMediator _mediator;

        public ReportGenerationJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task RunMonthlyReport()
        {
            var command = new GenerateAutoReportInstanceCommand(DateTime.Now);
            await _mediator.Send(command);
        }
    }
}
