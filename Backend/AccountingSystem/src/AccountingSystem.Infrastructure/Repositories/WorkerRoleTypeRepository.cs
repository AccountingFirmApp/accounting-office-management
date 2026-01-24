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
    public class WorkerRoleTypeRepository : IWorkerRoleTypeRepository
    {
        private AccountingDbContext context;

        public WorkerRoleTypeRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public Task<Workerroletype> AddAsync(Workerroletype entity)
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

        public Task<IEnumerable<Workerroletype>> FindAsync(Expression<Func<Workerroletype, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Workerroletype>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Workerroletype?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Workerroletype?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Workerroletype entity)
        {
            throw new NotImplementedException();
        }
    }
}
