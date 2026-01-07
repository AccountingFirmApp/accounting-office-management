using AccountingSystem.Domain.Entities;
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
    public class TaskTypeRepository:ITaskTypeRepository
    {
        private AccountingDbContext context;

        public TaskTypeRepository(AccountingDbContext context)
        {
            this.context = context;
        }

        public Task<Tasktype> AddAsync(Tasktype entity)
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

        public Task<IEnumerable<Tasktype>> FindAsync(Expression<Func<Tasktype, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tasktype>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tasktype>> GetByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        public Task<Tasktype?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Tasktype entity)
        {
            throw new NotImplementedException();
        }
    }
}
