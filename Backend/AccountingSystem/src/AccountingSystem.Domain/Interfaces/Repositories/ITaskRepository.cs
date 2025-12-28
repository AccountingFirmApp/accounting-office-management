using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<AccountingSystem.Domain.Entities.Task>
    {
        // חיפושים לפי מאפיינים
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksByWorkerIdAsync(Guid workerId);
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksByStatusAsync(string status);
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksByTaskTypeIdAsync(Guid taskTypeId);

        // חיפושים לפי תקופה
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksByPeriodAsync(DateTime period);
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);

        // משימות שדורשות תשומת לב
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetOverdueTasksAsync();
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetTasksDueInNextDaysAsync(int days);
        Task<IEnumerable<AccountingSystem.Domain.Entities.Task>> GetPendingTasksAsync();
    }
}