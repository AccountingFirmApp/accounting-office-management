using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    /// <summary>
    /// ממשק לפעולות ספציפיות של חברות
    /// יורש מIGenericRepository ומוסיף פעולות ייחודיות
    /// </summary>
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        // ========== קריאה עם קשרים (Eager Loading) ==========

        /// <summary>
        /// תביא לי חברה + כל אנשי הקשר שלה
        /// </summary>
        System.Threading.Tasks.Task AddAsync(Company company);
        public Task<List<Company?>> GetByIdWorkerAsync(int workerId);

        System.Threading.Tasks.Task<Company?> GetCompanyWithContactsAsync(int companyId);

        /// <summary>
        /// תביא לי חברה + כל העובדים שעובדים בה
        /// </summary>
        System.Threading.Tasks.Task<Company?> GetCompanyWithWorkersAsync(int companyId);

        /// <summary>
        /// תביא לי חברה + כל הגדרות הדוחות שלה
        /// </summary>
        System.Threading.Tasks.Task<Company?> GetCompanyWithReportConfigsAsync(int companyId);

        /// <summary>
        /// תביא לי חברה + הכל (אנשי קשר, עובדים, הגדרות דוחות)
        /// </summary>
        System.Threading.Tasks.Task<Company?> GetCompanyWithAllDetailsAsync(int companyId);

        // ========== חיפושים ==========

        /// <summary>
        /// תביא לי את כל החברות של משרד מסוים
        /// למה? כי משרד רוצה לראות רק את החברות שלו!
        /// </summary>
       System.Threading.Tasks.Task<IEnumerable<Company>> GetCompaniesByFirmIdAsync(int firmId);

        /// <summary>
        /// תביא לי רק חברות פעילות
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Company>> GetActiveCompaniesAsync();

        /// <summary>
        /// תביא לי רק חברות לא פעילות
        /// </summary>
        System.Threading.Tasks.Task<IEnumerable<Company>> GetInactiveCompaniesAsync();

        // ========== בדיקות ==========

        /// <summary>
        /// בדיקה: האם ח.פ/ע.מ הזה כבר קיים במערכת?
        /// למה? כדי למנוע חברות כפולות!
        /// excludeCompanyId = אל תבדוק את החברה הזאת (שימושי בעדכון)
        /// </summary>
        System.Threading.Tasks.Task<bool> TaxIdExistsAsync(string taxId, int? excludeCompanyId = null);
        Task<IEnumerable<Company>> GetAllByFirmIdAsync(int firmId);
    }
}