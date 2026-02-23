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
    public class ReportTypeRepository : IReportTypeRepository
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

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Reporttypes.AnyAsync(c => c.Id == id);
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

        public async Task<List<Reporttype>> GetToEdit()
        {
            List<Reporttype> req = await context.Reporttypes.ToListAsync();
            List<Reporttype> res =req.Where(x => x.Shortcode == "VAT" || x.Shortcode == "WHT" || x.Shortcode == "ITA_ADV" || x.Shortcode == "VAT_DET").ToList();
            return res; 
        }

        public Task UpdateAsync(Reporttype entity)
        {
            throw new NotImplementedException();
        }

     
    }
}
