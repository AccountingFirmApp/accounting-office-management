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
        System.Threading.Tasks.Task AddAsync(Worker worker);

        // ========== קריאה עם קשרים ==========

        /// <summary>
        /// תביא לי עובד + התפקיד שלו
        /// </summary>
        System.Threading.Tasks.Task<Worker?> GetWorkerWithRoleAsync(int workerId);

        /// <summary>
        /// תביא לי עובד + כל החברות שהוא עובד בהן
        /// </summary>
        System.Threading.Tasks.Task<Worker?> GetWorkerWithCompaniesAsync(int workerId);

        // ========== חיפושים ==========

        /// <summary>
        /// תביא לי את כל העובדים של משרד מסוים
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetWorkersByFirmIdAsync(int firmId);

        /// <summary>
        /// תביא לי את כל העובדים שעובדים בחברה מסוימת
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(int companyId);

        /// <summary>
        /// תביא לי רק עובדים פעילים
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetActiveWorkersAsync();

        /// <summary>
        /// תביא לי את כל העובדים עם תפקיד מסוים
        /// דוגמה: כל המנהלים
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Worker>> GetWorkersByRoleIdAsync(int roleId);

        // ========== בדיקות ==========

        /// <summary>
        /// בדיקה: האם האימייל הזה כבר תפוס?
        /// חשוב! כל אימייל חייב להיות ייחודי
        /// </summary>
        System.Threading.Tasks.Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null);



    }
}