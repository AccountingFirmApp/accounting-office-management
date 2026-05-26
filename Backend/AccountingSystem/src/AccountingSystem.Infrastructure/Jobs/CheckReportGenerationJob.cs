using AccountingSystem.Application.Queries;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
namespace AccountingSystem.Infrastructure.Jobs
{
    public class CheckReportGenerationJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CheckReportGenerationJob> _logger;


        public CheckReportGenerationJob(IMediator mediator, ILogger<CheckReportGenerationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [DisableConcurrentExecution(timeoutInSeconds: 300)]
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task RunDailyCheckReport()
        {
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
