using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AccountingSystem.Infrastructure.Repositories
{
   
    public class ReportInstanceRepository : IReportInstanceRepository
    {
        private readonly AccountingDbContext _context;
        private readonly ILogger<ReportInstanceRepository> _logger;

        public ReportInstanceRepository(AccountingDbContext context, ILogger<ReportInstanceRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        // ========== פונקציות בסיסיות מ-IGenericRepository ==========

        public async Task<Reportinstance?> GetByIdAsync(int id)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reportinstance>> GenerateReportsAsync(DateTime date)
        {
            var connection = _context.Database.GetDbConnection();
            try
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM generate_monthly_report_instances(@p_run_date)";
                command.Parameters.Add(new NpgsqlParameter("@p_run_date", NpgsqlTypes.NpgsqlDbType.Date) { Value = date });
                using var reader = await command.ExecuteReaderAsync();

                var iId            = reader.GetOrdinal("id");
                var iConfigId      = reader.GetOrdinal("configid");
                var iPeriod        = reader.GetOrdinal("period");
                var iAmount        = reader.GetOrdinal("amount");
                var iStatus        = reader.GetOrdinal("status");
                var iPaymentMethod = reader.GetOrdinal("paymentmethod");
                var iReceiptDate   = reader.GetOrdinal("receiptdate");
                var iReportedDate  = reader.GetOrdinal("reporteddate");
                var iPaidDate      = reader.GetOrdinal("paiddate");
                var iComments      = reader.GetOrdinal("comments");
                var iCreatedAt     = reader.GetOrdinal("createdat");
                var iUpdatedAt     = reader.GetOrdinal("updatedat");

                var res = new List<Reportinstance>();
                while (await reader.ReadAsync())
                {
                    res.Add(new Reportinstance
                    {
                        Id            = reader.GetInt32(iId),
                        Configid      = reader.GetInt32(iConfigId),
                        Period        = DateOnly.FromDateTime(reader.GetDateTime(iPeriod)),
                        Amount        = reader.IsDBNull(iAmount) ? null : reader.GetDecimal(iAmount),
                        Status        = reader.IsDBNull(iStatus) ? null : Enum.Parse<ReportStatus>(reader.GetString(iStatus)),
                        PaymentMethod = reader.IsDBNull(iPaymentMethod) ? null : Enum.Parse<PaymentMethod>(reader.GetString(iPaymentMethod)),
                        Receiptdate   = reader.IsDBNull(iReceiptDate) ? null : DateOnly.FromDateTime(reader.GetDateTime(iReceiptDate)),
                        Reporteddate  = reader.IsDBNull(iReportedDate) ? null : DateOnly.FromDateTime(reader.GetDateTime(iReportedDate)),
                        Paiddate      = reader.IsDBNull(iPaidDate) ? null : DateOnly.FromDateTime(reader.GetDateTime(iPaidDate)),
                        Comments      = reader.IsDBNull(iComments) ? null : reader.GetString(iComments),
                        Createdat     = reader.IsDBNull(iCreatedAt) ? null : reader.GetDateTime(iCreatedAt),
                        Updatedat     = reader.IsDBNull(iUpdatedAt) ? null : reader.GetDateTime(iUpdatedAt)
                    });
                }
                return res;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<bool> CheckReportsAsync()
        {
            var connection = _context.Database.GetDbConnection();
            try
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT check_report_health()";
                var result = await command.ExecuteScalarAsync();
                return result is true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return false;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
       

        public async Task<IEnumerable<Reportinstance>> GetAllAsync()
        {
            return await _context.Reportinstances

                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                        .ThenInclude(co => co.Companyworkers
                            .Where(cw => cw.Isactive == true))
                                .ThenInclude(cw => cw.Worker)

                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)

                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)

                .Where(r => r.Config.Isactive == true) 

                .AsNoTracking()
                .ToListAsync();
        }



        /// <summary>
        /// תביא לי דוחות לפי תנאי
        /// דוגמה: FindAsync(r => r.Status == ReportStatus.Pending)
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(predicate)
                .ToListAsync();
        }

        /// <summary>
        /// תעדכן דוח קיים
        /// </summary>
        public Task UpdateAsync(Reportinstance entity)
        {
            _context.Reportinstances.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// תמחק דוח לפי ID
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Reportinstances.FindAsync(id);
            if (entity != null)
            {
                _context.Reportinstances.Remove(entity);
            }
        }


        public async Task DeleteByConfigIdAsync(int configId)
        {
            var instances = await _context.Reportinstances
                .Where(r => r.Configid == configId)
                .ToListAsync();

            _context.Reportinstances.RemoveRange(instances);
        }

        public async Task DeleteByConfigIdsAsync(List<int> configIds)
        {
            var instances = await _context.Reportinstances
                .Where(r => configIds.Contains(r.Configid))
                .ToListAsync();

            _context.Reportinstances.RemoveRange(instances);
        }

        /// <summary>
        /// האם דוח קיים?
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Reportinstances.AnyAsync(r => r.Id == id);
        }

        /// <summary>
        /// כמה דוחות יש?
        /// </summary>
        public async Task<int> CountAsync(Func<object, bool> value)
        {
            return await _context.Reportinstances.CountAsync();
        }

        // ========== חיפושים לפי חברה ==========

        /// <summary>
        /// תביא לי את כל הדוחות של חברה
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Config.Companyid == companyId)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי את כל הדוחות של הגדרה מסוימת
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Configid == configId)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        // ========== חיפושים לפי סטטוס ==========

        /// <summary>
        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
        {
            if (Enum.TryParse<AccountingSystem.Domain.Enums.ReportStatus>(status, out var reportStatus))
            {
                return await _context.Reportinstances
                    .Include(r => r.Config)
                        .ThenInclude(c => c.Company)
                    .Include(r => r.Config)
                        .ThenInclude(c => c.Reporttype)
                    .Where(r => r.Status == reportStatus)
                    .OrderByDescending(r => r.Period)
                    .ToListAsync();
            }

            return new List<Reportinstance>();
        }

        /// <summary>
        /// תביא לי דוחות שממתינים (Pending)
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
        {
            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Where(r => r.Status == AccountingSystem.Domain.Enums.ReportStatus.Pending)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        // ========== חיפושים לפי תקופה ==========

        /// <summary>
        /// תביא לי דוחות של תקופה מסוימת
        /// דוגמה: כל הדוחות של 01/2024
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
        {
            var dateOnly = DateOnly.FromDateTime(period);
            var year = dateOnly.Year;
            var month = dateOnly.Month;

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Where(r => r.Period.Year == year && r.Period.Month == month)
                .OrderBy(r => r.Config.Company.Name)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי דוחות בטווח תאריכים
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Where(r => r.Period >= startDateOnly && r.Period <= endDateOnly)
                .OrderByDescending(r => r.Period)
                .ToListAsync();
        }

        // ========== דוחות שדורשים תשומת לב! ==========

        /// <summary>
        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
        /// זה קריטי! צריך להתריע על דוחות באיחור
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Period < today &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
                .OrderBy(r => r.Period)
                .ToListAsync();
        }

        /// <summary>
        /// תביא לי דוחות שצריך להגיש בעוד X ימים
        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
        /// </summary>
        public async Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var futureDate = today.AddDays(days);

            return await _context.Reportinstances
                .Include(r => r.Config)
                    .ThenInclude(c => c.Company)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Reporttype)
                .Include(r => r.Config)
                    .ThenInclude(c => c.Frequency)
                .Where(r => r.Period >= today &&
                           r.Period <= futureDate &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Reported &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Approved &&
                           r.Status != AccountingSystem.Domain.Enums.ReportStatus.Paid)
                .OrderBy(r => r.Period)
                .ToListAsync();
        }

        public async Task AddAsync(Reportinstance entity)
        {
            await _context.Reportinstances.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

     
    }
}