//using AccountingSystem.Domain.Entities;
//using AccountingSystem.Domain.Interfaces.Repositories;
//using AccountingSystem.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using TaskEntity = AccountingSystem.Domain.Entities.Task;

//namespace AccountingSystem.Infrastructure.Repositories
//{
//    public class TaskRepository : ITaskRepository
//    {
//        private readonly AccountingDbContext _context;
//        private readonly DbSet<Domain.Entities.Task> _dbSet;

//        public TaskRepository(AccountingDbContext context)
//        {
//            _context = context;
//            _dbSet = context.Tasks;
//        }

//        // ==================== פעולות בסיסיות ====================

//        //public async Task<Domain.Entities.Task?> GetByIdAsync(int id)
//        //{
//        //    return await _dbSet.FindAsync(id);
//        //}
//        public async Task<TaskEntity?> GetByIdAsync(int id)
//        {
//            return await _dbSet
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .Include(t => t.Assignedworker)
//                .FirstOrDefaultAsync(t => t.Id == id);
//        }

//        public System.Threading.Tasks.Task UpdateAsync(TaskEntity task)
//        {
//            _dbSet.Update(task);
//            return System.Threading.Tasks.Task.CompletedTask;
//        }
//        public async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
//        {
//            return await _dbSet
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .Include(t => t.Assignedworker)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> FindAsync(Expression<Func<Domain.Entities.Task, bool>> predicate)
//        {
//            return await _dbSet.Where(predicate).ToListAsync();
//        }

//        public async Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task entity)
//        {
//            await _dbSet.AddAsync(entity);
//            return entity;
//        }

//        //public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task entity)
//        //{
//        //    _dbSet.Update(entity);
//        //    await System.Threading.Tasks.Task.CompletedTask;
//        //}

//        public async System.Threading.Tasks.Task DeleteAsync(int id)
//        {
//            var entity = await GetByIdAsync(id);
//            if (entity != null)
//            {
//                _dbSet.Remove(entity);
//            }
//        }

//        public async Task<bool> ExistsAsync(int id)
//        {
//            return await _dbSet.AnyAsync(t => t.Id == id);
//        }

//        public async Task<int> CountAsync(Func<object, bool> predicate)
//        {
//            return await _dbSet.CountAsync();
//        }

//        // ==================== פעולות ייחודיות למשימות ====================

//        /// <summary>
//        /// קבלת כל המשימות של חברה מסוימת
//        /// זה מה שאת צריכה! 🎯
//        /// </summary>
//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByCompanyIdAsync(int companyId)
//        {
//            return await _dbSet
//                .Where(t => t.Companyid == companyId)
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .Include(t => t.Assignedworker)
//                .OrderBy(t => t.Duedate)  // ממוין לפי תאריך יעד
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByWorkerIdAsync(int workerId)
//        {
//            return await _dbSet
//                .Where(t => t.Assignedworkerid == workerId)
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .OrderBy(t => t.Duedate)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByStatusAsync(string status)
//        {
//            return await _dbSet
//                .Where(t => t.Status.ToString() == status)
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .Include(t => t.Assignedworker)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByTaskTypeIdAsync(int taskTypeId)
//        {
//            return await _dbSet
//                .Where(t => t.Tasktypeid == taskTypeId)
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByPeriodAsync(DateTime period)
//        {
//            var periodDateOnly = DateOnly.FromDateTime(period);
//            return await _dbSet
//                .Where(t => t.Period == periodDateOnly)
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
//        {
//            var startDateOnly = DateOnly.FromDateTime(startDate);
//            var endDateOnly = DateOnly.FromDateTime(endDate);

//            return await _dbSet
//                .Where(t => t.Period >= startDateOnly && t.Period <= endDateOnly)
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync()
//        {
//            var today = DateOnly.FromDateTime(DateTime.Today);
//            return await _dbSet
//                .Where(t => t.Duedate < today && t.Status.ToString() == "Pending")
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .Include(t => t.Assignedworker)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksDueInNextDaysAsync(int days)
//        {
//            var today = DateOnly.FromDateTime(DateTime.Today);
//            var futureDate = today.AddDays(days);

//            return await _dbSet
//                .Where(t => t.Duedate >= today && t.Duedate <= futureDate && t.Status.ToString() == "Pending")
//                .Include(t => t.Company)
//                .Include(t => t.Tasktype)
//                .Include(t => t.Assignedworker)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Domain.Entities.Task>> GetPendingTasksAsync()
//        {
//            return await GetTasksByStatusAsync("Pending");
//        }

//    }
//}
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TaskEntity = AccountingSystem.Domain.Entities.Task;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<TaskEntity> _dbSet;

        public TaskRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.Tasks;
        }

        // ==================== פעולות בסיסיות ====================

        public async System.Threading.Tasks.Task<TaskEntity?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public System.Threading.Tasks.Task UpdateAsync(TaskEntity task)
        {
            _dbSet.Update(task);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> FindAsync(Expression<Func<TaskEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async System.Threading.Tasks.Task<TaskEntity> AddAsync(TaskEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async System.Threading.Tasks.Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task<int> CountAsync(Func<object, bool> predicate)
        {
            return await _dbSet.CountAsync();
        }

        // ==================== פעולות ייחודיות למשימות ====================

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(t => t.Companyid == companyId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .OrderBy(t => t.Duedate)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByWorkerIdAsync(int workerId)
        {
            return await _dbSet
                .Where(t => t.Assignedworkerid == workerId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .OrderBy(t => t.Duedate)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByStatusAsync(string status)
        {
            return await _dbSet
                .Where(t => t.Status.ToString() == status)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByTaskTypeIdAsync(int taskTypeId)
        {
            return await _dbSet
                .Where(t => t.Tasktypeid == taskTypeId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByPeriodAsync(DateTime period)
        {
            var periodDateOnly = DateOnly.FromDateTime(period);
            return await _dbSet
                .Where(t => t.Period == periodDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(t => t.Period >= startDateOnly && t.Period <= endDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetOverdueTasksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _dbSet
                .Where(t => t.Duedate < today && t.Status.ToString() == "Pending")
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksDueInNextDaysAsync(int days)
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

        public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetPendingTasksAsync()
        {
            return await GetTasksByStatusAsync("Pending");
        }
    }
}