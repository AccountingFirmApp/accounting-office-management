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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<Company> _dbSet;

        public CompanyRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.Companies;
        }

        // ==================== פעולות בסיסיות ====================

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Firm)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> FindAsync(Expression<Func<Company, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<Company> AddAsync(Company entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async System.Threading.Tasks.Task UpdateAsync(Company entity)
        {
            _dbSet.Update(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }

        public async Task<int> CountAsync(Func<object, bool> predicate)
        {
            return await _dbSet.CountAsync();
        }

        // ==================== פעולות ייחודיות לCompany ====================

        public async Task<Company?> GetCompanyWithContactsAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Companycontacts)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyWithWorkersAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Companyworkers)
                    .ThenInclude(cw => cw.Worker)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyWithReportConfigsAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Companyreportconfigs)
                    .ThenInclude(crc => crc.Reporttype)
                .Include(c => c.Companyreportconfigs)
                    .ThenInclude(crc => crc.Frequency)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyWithAllDetailsAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Firm)
                .Include(c => c.Companycontacts)
                .Include(c => c.Companyworkers)
                    .ThenInclude(cw => cw.Worker)
                .Include(c => c.Companyreportconfigs)
                    .ThenInclude(crc => crc.Reporttype)
                .Include(c => c.Companyreportconfigs)
                    .ThenInclude(crc => crc.Frequency)
                .Include(c => c.Tasks)  // ← זה חשוב למשימות!
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<IEnumerable<Company>> GetCompaniesByFirmIdAsync(int firmId)
        {
            return await _dbSet
                .Where(c => c.Firmid == firmId)
                .Include(c => c.Firm)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetActiveCompaniesAsync()
        {
            return await _dbSet
                .Where(c => c.Isactive == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetInactiveCompaniesAsync()
        {
            return await _dbSet
                .Where(c => c.Isactive == false)
                .ToListAsync();
        }

        public async Task<bool> TaxIdExistsAsync(string taxId, int? excludeCompanyId = null)
        {
            var query = _dbSet.Where(c => c.Taxid == taxId);

            if (excludeCompanyId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCompanyId.Value);
            }

            return await query.AnyAsync();
        }

        System.Threading.Tasks.Task IGenericRepository<Company>.AddAsync(Company entity)
        {
            throw new NotImplementedException();
        }
    }
}