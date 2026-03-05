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
        // שאילתות ספציפיות למשימות
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
        // פעולות ליצירה אוטומטית
        // ==========================================

        Task<List<TaskTypeConfiguration>> GetActiveConfigurationsByRecurrenceAsync(RecurrenceType recurrenceType);
        Task<TaskTypeConfiguration?> GetConfigurationByTaskTypeAsync(int taskTypeId);
        Task<List<Company>> GetActiveCompaniesAsync();
        Task<CompanyTaskTypeSettings?> GetCompanySettingsAsync(int companyId, int taskTypeId);
        Task<bool> TaskExistsAsync(int companyId, int taskTypeId, DateOnly period);
        Task<List<CompanyTask>> GetCompanyTasksAsync(int companyId, int? year = null, int? month = null, TaskStatus1? status = null);
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
        Task<List<Tasktype>> GetTaskTypesAsync();

        Task<TaskChecklistTemplate> GetChecklistTemplateByTaskTypeAsync(int taskTypeId);
        Task UpdateChecklistTemplateAsync(TaskChecklistTemplate template, List<TaskChecklistTemplateItem> newItems);
        Task DeleteByCompanyIdAsync(int companyId);
        Task SoftDeleteByCompanyIdAsync(int companyId);

    }
    }
