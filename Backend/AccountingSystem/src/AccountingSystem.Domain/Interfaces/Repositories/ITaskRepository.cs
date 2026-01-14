using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using TaskEntity = AccountingSystem.Domain.Entities.Task;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>
    {
        // ← הפונקציות החשובות לעדכון סטטוס
        System.Threading.Tasks.Task<TaskEntity?> GetByIdAsync(int id);
        System.Threading.Tasks.Task UpdateAsync(TaskEntity task);

        // חיפושים לפי מאפיינים
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByWorkerIdAsync(int workerId);
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByStatusAsync(string status);
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByTaskTypeIdAsync(int taskTypeId);

        // חיפושים לפי תקופה
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByPeriodAsync(DateTime period);
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);

        // משימות שדורשות תשומת לב
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetOverdueTasksAsync();
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksDueInNextDaysAsync(int days);
        System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetPendingTasksAsync();
    }
}