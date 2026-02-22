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
    /// <summary>
    /// מימוש של פעולות Repository עבור דוחות
    /// </summary>
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

        /// <summary>
        /// תביא לי דוח לפי ID
        /// </summary>
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
            await connection.OpenAsync();
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM generate_monthly_report_instances(@p_run_date)";
                command.Parameters.Add(new NpgsqlParameter("@p_run_date", NpgsqlTypes.NpgsqlDbType.Date) { Value = date });
                using var reader = await command.ExecuteReaderAsync();

                var res = new List<Reportinstance>();
                while (await reader.ReadAsync())
                {
                    res.Add(new Reportinstance
                    {
                        Id = reader.GetInt32(0),
                        Configid = reader.GetInt32(1),
                        Period = DateOnly.FromDateTime(reader.GetDateTime(2)),
                        Amount = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                        Status = reader.IsDBNull(4) ? null : Enum.Parse<ReportStatus>(reader.GetString(4)),
                        PaymentMethod = reader.IsDBNull(5) ? null : Enum.Parse<PaymentMethod>(reader.GetString(5)),
                        Receiptdate = reader.IsDBNull(6) ? null : DateOnly.FromDateTime(reader.GetDateTime(6)),
                        Reporteddate = reader.IsDBNull(7) ? null : DateOnly.FromDateTime(reader.GetDateTime(7)),
                        Paiddate = reader.IsDBNull(8) ? null : DateOnly.FromDateTime(reader.GetDateTime(8)),
                        Comments = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Createdat = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
                        Updatedat = reader.IsDBNull(11) ? null : reader.GetDateTime(11)
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
            await connection.OpenAsync();
            try
            {
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
                            .ThenInclude(co => co.Companyworkers.Where(cw => cw.Isactive == true)) // 🔥 רק עובדות פעילות
                                .ThenInclude(cw => cw.Worker)
                    .Include(r => r.Config)
                        .ThenInclude(c => c.Reporttype)
                    .Include(r => r.Config)
                        .ThenInclude(c => c.Frequency)
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
            // המרה מ-string ל-Enum
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

        //Task IGenericRepository<Reportinstance>.AddAsync(Reportinstance entity)
        //{

        //    throw new NotImplementedException();
        //}

        //public async Task<Reportinstance> AddAsync(Reportinstance entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException(nameof(entity));
        //    }

        //    // הוספת תאריך יצירה אם לא הוגדר
        //    if (entity.Createdat == default)
        //    {
        //        entity.Createdat = DateTime.UtcNow;
        //    }

        //    await _context.Reportinstances.AddAsync(entity);
        //    await _context.SaveChangesAsync();

        //    return entity;
        //}

        //Task IGenericRepository<Reportinstance>.AddAsync(Reportinstance entity)
        //{
        //    return AddAsync(entity);
        //}

        public async Task AddAsync(Reportinstance entity)
        {
            await _context.Reportinstances.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

     
    }
}