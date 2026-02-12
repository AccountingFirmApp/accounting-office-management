using AccountingSystem.Application.Queries;
using Hangfire;
using MediatR;

namespace AccountingSystem.API.Jobs
{
    public class CheckReportGenerationJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;


        public CheckReportGenerationJob(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task RunDailyCheckReport()
        {
            var today = DateTime.Now.Day;
            if (today < 26) return;
            var query = new CheckReportInstanceQuery();
            var check = await _mediator.Send(query);
            if (!check)
            {
                _logger.LogWarning("Missing reports detected! Triggering generation...");
                BackgroundJob.Enqueue<ReportGenerationJob>(job => job.RunMonthlyReport());
            }

        }

    }
}
