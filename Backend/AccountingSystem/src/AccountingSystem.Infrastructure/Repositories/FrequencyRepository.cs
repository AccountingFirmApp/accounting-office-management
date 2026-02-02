using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        public Task AddAsync(Frequency entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Frequencies.AnyAsync(c => c.Id == id);

        }

        public Task<IEnumerable<Frequency>> FindAsync(Expression<Func<Frequency, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Frequency>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Frequency?> GetByIdAsync(int id)
        {
            return await context.Frequencies.FirstOrDefaultAsync(c => c.Id == id);

        }

        public Task<Frequency?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Frequency entity)
        {
            throw new NotImplementedException();
        }
    }
}
