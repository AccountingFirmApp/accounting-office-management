//using AccountingSystem.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace AccountingSystem.Domain.Interfaces.Repositories
//{
//    public interface ICompanyTaskRepository : IGenericRepository<CompanyTask>
//    {
//        // פעולות בסיסיות (מורחבות)
//        System.Threading.Tasks.Task<CompanyTask?> GetByIdAsync(int id);
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetAllAsync();

//        // חיפושים לפי מאפיינים
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId);
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByWorkerIdAsync(int workerId);
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByStatusAsync(string status);
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByTaskTypeIdAsync(int taskTypeId);

//        // חיפושים לפי תקופה
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByPeriodAsync(DateTime period);
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);

//        // משימות שדורשות תשומת לב
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetOverdueTasksAsync();
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksDueInNextDaysAsync(int days);
//        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetPendingTasksAsync();
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyTaskRepository
    {
        // ==========================================
        // פעולות בסיסיות (Generic Repository Pattern)
        // ==========================================

        Task<CompanyTask?> GetByIdAsync(int id);
        Task<IEnumerable<CompanyTask>> GetAllAsync();
        Task<IEnumerable<CompanyTask>> FindAsync(Expression<Func<CompanyTask, bool>> predicate);
        Task AddAsync(CompanyTask entity);
        Task UpdateAsync(CompanyTask entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();

        // ==========================================
        // שאילתות ספציפיות למשימות - מהממשק הישן
        // ==========================================

        Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId);
        Task<IEnumerable<CompanyTask>> GetTasksByWorkerIdAsync(int workerId);
        Task<IEnumerable<CompanyTask>> GetTasksByStatusAsync(string status);
        Task<IEnumerable<CompanyTask>> GetTasksByTaskTypeIdAsync(int taskTypeId);
        Task<IEnumerable<CompanyTask>> GetTasksByPeriodAsync(DateTime period);
        Task<IEnumerable<CompanyTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CompanyTask>> GetOverdueTasksAsync();
        Task<IEnumerable<CompanyTask>> GetTasksDueInNextDaysAsync(int days);
        Task<IEnumerable<CompanyTask>> GetPendingTasksAsync();

        // ==========================================
        // פעולות ליצירה אוטומטית - מהממשק החדש
        // ==========================================

        /// <summary>
        /// קבלת כל התצורות לפי תדירות
        /// </summary>
        Task<List<TaskTypeConfiguration>> GetActiveConfigurationsByRecurrenceAsync(RecurrenceType recurrenceType);

        /// <summary>
        /// קבלת תצורה לפי סוג משימה
        /// </summary>
        Task<TaskTypeConfiguration?> GetConfigurationByTaskTypeAsync(int taskTypeId);

        /// <summary>
        /// קבלת כל החברות הפעילות
        /// </summary>
        Task<List<Company>> GetActiveCompaniesAsync();

        /// <summary>
        /// קבלת הגדרות חברה לסוג משימה
        /// </summary>
        Task<CompanyTaskTypeSettings?> GetCompanySettingsAsync(int companyId, int taskTypeId);

        /// <summary>
        /// בדיקה האם משימה קיימת
        /// </summary>
        Task<bool> TaskExistsAsync(int companyId, int taskTypeId, DateOnly period);

        /// <summary>
        /// קבלת משימות של חברה עם פילטרים
        /// </summary>
        Task<List<CompanyTask>> GetCompanyTasksAsync(
            int companyId,
            int? year = null,
            int? month = null,
            TaskStatus1? status = null);

        /// <summary>
        /// יצירת מספר משימות בבת אחת
        /// </summary>
        Task<int> CreateTasksAsync(List<CompanyTask> tasks);

        // ==========================================
        // Checklist
        // ==========================================

        Task<TaskChecklistTemplate?> GetActiveChecklistTemplateAsync(int taskTypeId);
        Task<List<CompanyTaskChecklistItem>> GetTaskChecklistItemsAsync(int taskId);
        Task<CompanyTaskChecklistItem?> GetChecklistItemByIdAsync(int itemId);
        Task UpdateChecklistItemAsync(CompanyTaskChecklistItem item);
        Task<CompanyTaskChecklistItem> AddChecklistItemAsync(CompanyTaskChecklistItem item);
        Task DeleteChecklistItemAsync(int itemId);
    }
}