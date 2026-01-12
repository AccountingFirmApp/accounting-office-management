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
    public class ReportTypeRepository:IReportTypeRepository
    {
        private AccountingDbContext context;

        public ReportTypeRepository(AccountingDbContext context)
        {
            this.context = context;
        }

        public Task<Reporttype> AddAsync(Reporttype entity)
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

        public Task<IEnumerable<Reporttype>> FindAsync(Expression<Func<Reporttype, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reporttype>> GetAllAsync()
        {
            return await context.Reporttypes.ToListAsync();
        }

        public Task<Reporttype?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Reporttype?> GetByShortcodeAsync(string shortcode)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Reporttype entity)
        {
            throw new NotImplementedException();
        }
    }
}
