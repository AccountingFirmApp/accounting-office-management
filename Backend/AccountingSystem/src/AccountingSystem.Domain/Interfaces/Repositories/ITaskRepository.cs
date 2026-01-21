using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using TaskEntity = AccountingSystem.Domain.Entities.Task;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>
    {
        // ← הפונקציות החשובות לעדכון סטטוס
       AccountingSystem.Domain.Entities.Task GetByIdAsync(int id);
        AccountingSystem.Domain.Entities.Task UpdateAsync(TaskEntity task);

        // חיפושים לפי מאפיינים
        AccountingSystem.Domain.Entities.Task GetTasksByCompanyIdAsync(int companyId);
        AccountingSystem.Domain.Entities.Task GetTasksByWorkerIdAsync(int workerId);
        AccountingSystem.Domain.Entities.Task GetTasksByStatusAsync(string status);
        AccountingSystem.Domain.Entities.Task GetTasksByTaskTypeIdAsync(int taskTypeId);

        // חיפושים לפי תקופה
        AccountingSystem.Domain.Entities.Task GetTasksByPeriodAsync(DateTime period);
        AccountingSystem.Domain.Entities.Task GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate);

        // משימות שדורשות תשומת לב
        AccountingSystem.Domain.Entities.Task GetOverdueTasksAsync();
        AccountingSystem.Domain.Entities.Task GetTasksDueInNextDaysAsync(int days);
        AccountingSystem.Domain.Entities.Task GetPendingTasksAsync();
    }
}