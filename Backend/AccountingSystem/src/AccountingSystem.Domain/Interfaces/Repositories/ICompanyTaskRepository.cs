using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyTaskRepository : IGenericRepository<CompanyTask>
    {
        // פעולות בסיסיות (מורחבות)
        System.Threading.Tasks.Task<CompanyTask?> GetByIdAsync(int id);
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetAllAsync();

        // חיפושים לפי מאפיינים
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByWorkerIdAsync(int workerId);
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByStatusAsync(string status);
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByTaskTypeIdAsync(int taskTypeId);

        // חיפושים לפי תקופה
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByPeriodAsync(DateTime period);
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);

        // משימות שדורשות תשומת לב
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetOverdueTasksAsync();
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetTasksDueInNextDaysAsync(int days);
        System.Threading.Tasks.Task<IEnumerable<CompanyTask>> GetPendingTasksAsync();
    }
}