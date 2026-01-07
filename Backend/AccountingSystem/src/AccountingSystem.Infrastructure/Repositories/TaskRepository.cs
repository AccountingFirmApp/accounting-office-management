using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private AccountingDbContext context;

        public TaskRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> FindAsync(Expression<Func<Domain.Entities.Task, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Entities.Task?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetPendingTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByPeriodAsync(DateTime period)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByTaskTypeIdAsync(int taskTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByWorkerIdAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksDueInNextDaysAsync(int days)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task entity)
        {
            throw new NotImplementedException();
        }
    }
}
