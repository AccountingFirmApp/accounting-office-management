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

<<<<<<< HEAD
        public AccountingSystem.Domain.Entities.Task<Tasktype> AddAsync(Tasktype entity)
=======
        public System.Threading.Tasks.Task AddAsync(Tasktype entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Tasktype>> FindAsync(Expression<Func<Tasktype, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Tasktype>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Tasktype>> GetByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Tasktype?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Tasktype entity)
        {
            throw new NotImplementedException();
        }
    }
}
