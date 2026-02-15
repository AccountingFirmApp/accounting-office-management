using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyTaskRepository : ICompanyTaskRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<CompanyTask> _dbSet;

        public CompanyTaskRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.CompanyTasks;
        }


        public async Task<CompanyTask?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<CompanyTask>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> FindAsync(Expression<Func<CompanyTask, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(CompanyTask entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(CompanyTask entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task DeleteByCompanyIdAsync(int companyId)
        {
            var tasks = await _context.CompanyTasks
                .Where(t => t.Companyid == companyId)
                .ToListAsync();

            _context.CompanyTasks.RemoveRange(tasks);
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(t => t.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }


      
        public async Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(t => t.Companyid == companyId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .OrderBy(t => t.Duedate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByWorkerIdAsync(int workerId)
        {
            return await _dbSet
                .Where(t => t.Assignedworkerid == workerId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .OrderBy(t => t.Duedate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByStatusAsync(string status)
        {
            return await _dbSet
                .Where(t => t.Status.ToString() == status)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByTaskTypeIdAsync(int taskTypeId)
        {
            return await _dbSet
                .Where(t => t.Tasktypeid == taskTypeId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByPeriodAsync(DateTime period)
        {
            var periodDateOnly = DateOnly.FromDateTime(period);
            return await _dbSet
                .Where(t => t.Period == periodDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(t => t.Period >= startDateOnly && t.Period <= endDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetOverdueTasksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _dbSet
                .Where(t => t.Duedate < today && t.Status.ToString() == "Pending")
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksDueInNextDaysAsync(int days)
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

        public async Task<IEnumerable<CompanyTask>> GetPendingTasksAsync()
        {
            return await GetTasksByStatusAsync("Pending");
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}