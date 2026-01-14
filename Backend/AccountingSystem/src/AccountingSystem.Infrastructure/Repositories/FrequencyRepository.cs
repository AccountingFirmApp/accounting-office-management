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
    public class FrequencyRepository : IFrequencyRepository
    {
        private AccountingDbContext context;

        public FrequencyRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public System.Threading.Tasks.Task AddAsync(Frequency entity)
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

        public Task<IEnumerable<Frequency>> FindAsync(Expression<Func<Frequency, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Frequency>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Frequency?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Frequency?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Frequency entity)
        {
            throw new NotImplementedException();
        }
    }
}
