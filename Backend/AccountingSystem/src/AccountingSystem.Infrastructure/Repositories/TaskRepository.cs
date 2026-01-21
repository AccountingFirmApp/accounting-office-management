using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskEntity = AccountingSystem.Domain.Entities.AccountingSystem.Domain.Entities.Task;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<Domain.Entities.AccountingSystem.Domain.Entities.Task> _dbSet;

        public TaskRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.Tasks;
        }


        // ==================== פעולות בסיסיות ====================

        public async AccountingSystem.Domain.Entities.Task<Domain.Entities.AccountingSystem.Domain.Entities.Task?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> FindAsync(Expression<Func<Domain.Entities.AccountingSystem.Domain.Entities.Task, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
<<<<<<< HEAD

        public async AccountingSystem.Domain.Entities.Task<Domain.Entities.AccountingSystem.Domain.Entities.Task> AddAsync(Domain.Entities.AccountingSystem.Domain.Entities.Task entity)
=======
        public async System.Threading.Tasks.Task AddAsync(Domain.Entities.Task entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            await _dbSet.AddAsync(entity);
            // לא מחזירים כלום - רק Task
        }
        //public async Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    return entity;
        //}

        public async System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Domain.Entities.AccountingSystem.Domain.Entities.Task entity)
        {
            _dbSet.Update(entity);
            await System.Threading.Tasks.AccountingSystem.Domain.Entities.Task.CompletedTask;
        }

        public async System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(t => t.Id == id);
        }

        public async AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> predicate)
        {
            return await _dbSet.CountAsync();
        }

        // ==================== פעולות ייחודיות למשימות ====================

        /// <summary>
        /// קבלת כל המשימות של חברה מסוימת
        /// זה מה שאת צריכה! 🎯
        /// </summary>
        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(t => t.Companyid == companyId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .OrderBy(t => t.Duedate)  // ממוין לפי תאריך יעד
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksByWorkerIdAsync(int workerId)
        {
            return await _dbSet
                .Where(t => t.Assignedworkerid == workerId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .OrderBy(t => t.Duedate)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksByStatusAsync(string status)
        {
            return await _dbSet
                .Where(t => t.Status.ToString() == status)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksByTaskTypeIdAsync(int taskTypeId)
        {
            return await _dbSet
                .Where(t => t.Tasktypeid == taskTypeId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksByPeriodAsync(DateTime period)
        {
            var periodDateOnly = DateOnly.FromDateTime(period);
            return await _dbSet
                .Where(t => t.Period == periodDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(t => t.Period >= startDateOnly && t.Period <= endDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetOverdueTasksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _dbSet
                .Where(t => t.Duedate < today && t.Status.ToString() == "Pending")
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetTasksDueInNextDaysAsync(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var futureDate = today.AddDays(days);

            return await _dbSet
                .Where(t => t.Duedate >= today && t.Duedate <= futureDate && t.Status.ToString() == "Pending")
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Domain.Entities.AccountingSystem.Domain.Entities.Task>> GetPendingTasksAsync()
        {
            return await GetTasksByStatusAsync("Pending");
        }

        System.Threading.Tasks.Task IGenericRepository<TaskEntity>.AddAsync(TaskEntity entity)
        {
            return AddAsync(entity);
        }
    }
}