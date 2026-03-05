

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AccountingSystem.Application.Commands.Tasks;
using MediatR;

namespace AccountingSystem.Infrastructure.Jobs
{
    /// <summary>
    /// Background Service שרץ כל יום ובודק אם צריך ליצור משימות חדשות
    /// רץ אוטומטית כל 24 שעות
    /// </summary>
    public class AutomaticTaskGenerationJob : BackgroundService
    {
        private readonly ILogger<AutomaticTaskGenerationJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

        public AutomaticTaskGenerationJob(
            ILogger<AutomaticTaskGenerationJob> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🤖 [AutoTaskGen] Background Service התחיל - בדיקה כל 24 שעות");

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("⏰ [AutoTaskGen] התחלת בדיקה ליצירת משימות אוטומטית...");
                    await GenerateTasksIfNeeded(stoppingToken);
                    _logger.LogInformation("✅ [AutoTaskGen] בדיקה הסתיימה בהצלחה");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ [AutoTaskGen] שגיאה קריטית ביצירה אוטומטית של משימות");
                }

                _logger.LogInformation($"💤 [AutoTaskGen] ממתין {_checkInterval.TotalHours} שעות עד הבדיקה הבאה");
                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("🛑 [AutoTaskGen] Background Service נעצר");
        }

        private async Task GenerateTasksIfNeeded(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var now = DateTime.Now;
            var today = now.Day;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            _logger.LogInformation($"📅 [AutoTaskGen] תאריך נוכחי: {now:dd/MM/yyyy}, יום בחודש: {today}");

            // רק בימים 1-3 של החודש
            //if (today > 3)
            //{

            //    _logger.LogInformation($"⏭️ [AutoTaskGen] לא תחילת חודש (יום {today}) - מדלג על יצירת משימות");
            //    return;
            //}

            // ==========================================
            // 1. משימות חודשיות
            // ==========================================
            try
            {
                var monthlyResult = await mediator.Send(
                    new GenerateMonthlyTasksCommand
                    {
                        Year = currentYear,
                        Month = currentMonth
                    },
                    cancellationToken
                );

                _logger.LogInformation(
                    $"✅ [AutoTaskGen] נוצרו {monthlyResult.TasksCreated} משימות חודשיות חדשות"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [AutoTaskGen] שגיאה ביצירת משימות חודשיות");
            }

            // ==========================================
            // 2. משימות רבעוניות
            // ==========================================
            if (currentMonth % 3 == 1)
            {
                var quarter = (currentMonth - 1) / 3 + 1;

                try
                {
                    var quarterlyResult = await mediator.Send(
                        new GenerateQuarterlyTasksCommand
                        {
                            Year = currentYear,
                            Quarter = quarter
                        },
                        cancellationToken
                    );

                    _logger.LogInformation(
                        $"✅ [AutoTaskGen] נוצרו {quarterlyResult.TasksCreated} משימות רבעוניות חדשות"
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"❌ [AutoTaskGen] שגיאה ביצירת משימות רבעוניות לרבעון {quarter}");
                }
            }

            // ==========================================
            // 3. משימות שנתיות
            // ==========================================
            if (currentMonth == 1)
            {
                try
                {
                    var yearlyResult = await mediator.Send(
                        new GenerateYearlyTasksCommand
                        {
                            Year = currentYear
                        },
                        cancellationToken
                    );

                    _logger.LogInformation(
                        $"✅ [AutoTaskGen] נוצרו {yearlyResult.TasksCreated} משימות שנתיות חדשות"
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"❌ [AutoTaskGen] שגיאה ביצירת משימות שנתיות ל-{currentYear}");
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🛑 [AutoTaskGen] מקבל פקודת עצירה...");
            await base.StopAsync(cancellationToken);
        }
    }
}
