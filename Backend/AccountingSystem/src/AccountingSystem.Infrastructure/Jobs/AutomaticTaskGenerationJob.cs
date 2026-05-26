using AccountingSystem.Application.Commands.Tasks;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountingSystem.Infrastructure.Jobs
{
    public class AutomaticTaskGenerationJob
    {
        private static readonly TimeZoneInfo IsraelTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");

        private readonly IMediator _mediator;
        private readonly ILogger<AutomaticTaskGenerationJob> _logger;

        public AutomaticTaskGenerationJob(
            IMediator mediator,
            ILogger<AutomaticTaskGenerationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task RunMonthlyTaskGeneration(CancellationToken cancellationToken = default)
        {
            var israelNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IsraelTimeZone);
            var year = israelNow.Year;
            var month = israelNow.Month;

            _logger.LogInformation(
                "Starting automatic task generation for {Year}-{Month:D2}", year, month);

            await TryGenerateAsync(
                "monthly",
                new GenerateMonthlyTasksCommand { Year = year, Month = month },
                cancellationToken);

            if (month % 3 == 1)
            {
                var quarter = (month - 1) / 3 + 1;
                await TryGenerateAsync(
                    $"quarterly Q{quarter}",
                    new GenerateQuarterlyTasksCommand { Year = year, Quarter = quarter },
                    cancellationToken);
            }

            if (month == 1)
            {
                await TryGenerateAsync(
                    "yearly",
                    new GenerateYearlyTasksCommand { Year = year },
                    cancellationToken);
            }

            if (month % 2 != 0)
            {
                await TryGenerateAsync(
                    "bi-monthly",
                    new GenerateBiMonthlyTasksCommand { Year = year, Month = month },
                    cancellationToken);
            }

            _logger.LogInformation("Automatic task generation completed");
        }

        private async Task TryGenerateAsync(
            string category,
            IRequest<GenerateTasksResult> command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                _logger.LogInformation(
                    "Created {Count} {Category} tasks", result.TasksCreated, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate {Category} tasks", category);
            }
        }
    }
}
