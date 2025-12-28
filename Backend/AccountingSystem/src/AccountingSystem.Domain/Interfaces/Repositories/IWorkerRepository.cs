using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    /// <summary>
    /// ממשק לפעולות ספציפיות של עובדים
    /// </summary>
    public interface IWorkerRepository : IGenericRepository<Worker>
    {
        // ========== קריאה עם קשרים ==========

        /// <summary>
        /// תביא לי עובד + התפקיד שלו
        /// </summary>
        Task<Worker?> GetWorkerWithRoleAsync(Guid workerId);

        /// <summary>
        /// תביא לי עובד + כל החברות שהוא עובד בהן
        /// </summary>
        Task<Worker?> GetWorkerWithCompaniesAsync(Guid workerId);

        // ========== חיפושים ==========

        /// <summary>
        /// תביא לי את כל העובדים של משרד מסוים
        /// </summary>
        Task<IEnumerable<Worker>> GetWorkersByFirmIdAsync(Guid firmId);

        /// <summary>
        /// תביא לי את כל העובדים שעובדים בחברה מסוימת
        /// </summary>
        Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(Guid companyId);

        /// <summary>
        /// תביא לי רק עובדים פעילים
        /// </summary>
        Task<IEnumerable<Worker>> GetActiveWorkersAsync();

        /// <summary>
        /// תביא לי את כל העובדים עם תפקיד מסוים
        /// דוגמה: כל המנהלים
        /// </summary>
        Task<IEnumerable<Worker>> GetWorkersByRoleIdAsync(Guid roleId);

        // ========== בדיקות ==========

        /// <summary>
        /// בדיקה: האם האימייל הזה כבר תפוס?
        /// חשוב! כל אימייל חייב להיות ייחודי
        /// </summary>
        Task<bool> EmailExistsAsync(string email, Guid? excludeWorkerId = null);
    }
}