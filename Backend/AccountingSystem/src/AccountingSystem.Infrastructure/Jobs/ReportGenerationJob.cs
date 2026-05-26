using AccountingSystem.Application.Commands;
using Hangfire;
using MediatR;

namespace AccountingSystem.Infrastructure.Jobs
{
    public class ReportGenerationJob
    {
        private static readonly TimeZoneInfo IsraelTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");

        private readonly IMediator _mediator;

        public ReportGenerationJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task RunMonthlyReport()
        {
            var israelNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IsraelTimeZone);
            var command = new GenerateAutoReportInstanceCommand(israelNow);
            await _mediator.Send(command);
        }
    }
}
