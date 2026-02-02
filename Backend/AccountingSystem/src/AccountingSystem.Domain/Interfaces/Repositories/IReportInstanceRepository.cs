using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    /// <summary>
    /// ממשק לפעולות ספציפיות של דוחות
    /// </summary>
    public interface IReportInstanceRepository : IGenericRepository<Reportinstance>
    {
        // ========== חיפושים לפי חברה ==========

        /// <summary>
        /// תביא לי את כל הדוחות של חברה
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId);

        /// <summary>
        /// תביא לי את כל הדוחות של הגדרה מסוימת
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId);

        // ========== חיפושים לפי סטטוס ==========

        /// <summary>
        /// תביא לי דוחות לפי סטטוס (Draft, Submitted, Approved...)
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status);

        /// <summary>
        /// תביא לי דוחות שממתינים (Pending)
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetPendingReportsAsync();

        // ========== חיפושים לפי תקופה ==========

        /// <summary>
        /// תביא לי דוחות של תקופה מסוימת
        /// דוגמה: כל הדוחות של 01/2024
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period);

        /// <summary>
        /// תביא לי דוחות בטווח תאריכים
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate);

        // ========== דוחות שדורשים תשומת לב! ==========

        /// <summary>
        /// תביא לי דוחות שפספסו את תאריך היעד! (OVERDUE)
        /// זה קריטי! צריך להתריע על דוחות באיחור
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync();

        /// <summary>
        /// תביא לי דוחות שצריך להגיש בעוד X ימים
        /// למשל: תביא לי דוחות שצריך להגיש בשבוע הקרוב
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days);
        System.Threading.Tasks.Task AddAsync(Reportinstance report);

    }
}