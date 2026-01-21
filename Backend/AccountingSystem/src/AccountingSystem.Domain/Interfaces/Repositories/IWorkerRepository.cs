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
        AccountingSystem.Domain.Entities.Task<Worker?> GetWorkerWithRoleAsync(int workerId);

        /// <summary>
        /// תביא לי עובד + כל החברות שהוא עובד בהן
        /// </summary>
        AccountingSystem.Domain.Entities.Task<Worker?> GetWorkerWithCompaniesAsync(int workerId);

        // ========== חיפושים ==========

        /// <summary>
        /// תביא לי את כל העובדים של משרד מסוים
        /// </summary>
        AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetWorkersByFirmIdAsync(int firmId);

        /// <summary>
        /// תביא לי את כל העובדים שעובדים בחברה מסוימת
        /// </summary>
        AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(int companyId);

        /// <summary>
        /// תביא לי רק עובדים פעילים
        /// </summary>
        AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetActiveWorkersAsync();

        /// <summary>
        /// תביא לי את כל העובדים עם תפקיד מסוים
        /// דוגמה: כל המנהלים
        /// </summary>
        AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetWorkersByRoleIdAsync(int roleId);

        // ========== בדיקות ==========

        /// <summary>
        /// בדיקה: האם האימייל הזה כבר תפוס?
        /// חשוב! כל אימייל חייב להיות ייחודי
        /// </summary>
        AccountingSystem.Domain.Entities.Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null);



    }
}