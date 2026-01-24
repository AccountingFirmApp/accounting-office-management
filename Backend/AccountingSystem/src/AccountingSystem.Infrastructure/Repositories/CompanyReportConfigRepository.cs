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
    public class CompanyReportConfigRepository : ICompanyReportConfigRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<Companyreportconfig> _dbSet;

        public CompanyReportConfigRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.Companyreportconfigs;
        }

        // ==================== פעולות בסיסיות ====================

        public async Task<Companyreportconfig?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Companyreportconfig>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

        public async Task<IEnumerable<Companyreportconfig>> FindAsync(Expression<Func<Companyreportconfig, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

        public async Task AddAsync(Companyreportconfig entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(Companyreportconfig entity)
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

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }

     
        // ==================== פעולות ייחודיות ====================

        /// <summary>
        /// קבלת כל ההגדרות של חברה מסוימת
        /// </summary>
        public async Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(c => c.Companyid == companyId)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .OrderBy(c => c.Reporttype.Name)
                .ToListAsync();
        }

        /// <summary>
        /// קבלת כל ההגדרות הפעילות בלבד
        /// </summary>
        public async Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync()
        {
            return await _dbSet
                .Where(c => c.Isactive == true)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

        /// <summary>
        /// קבלת הגדרות פעילות לפי חברה
        /// </summary>
        public async Task<IEnumerable<Companyreportconfig>> GetActiveConfigsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(c => c.Companyid == companyId && c.Isactive == true)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

        /// <summary>
        /// קבלת הגדרות לפי סוג דיווח
        /// </summary>
        public async Task<IEnumerable<Companyreportconfig>> GetConfigsByReportTypeIdAsync(int reportTypeId)
        {
            return await _dbSet
                .Where(c => c.Reporttypeid == reportTypeId)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

        /// <summary>
        /// קבלת הגדרות לפי תדירות
        /// </summary>
        public async Task<IEnumerable<Companyreportconfig>> GetConfigsByFrequencyIdAsync(int frequencyId)
        {
            return await _dbSet
                .Where(c => c.Frequencyid == frequencyId)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

        /// <summary>
        /// בדיקה אם קיימת כבר הגדרה לחברה ולסוג דיווח מסוים
        /// </summary>
        public async Task<bool> ConfigExistsAsync(int companyId, int reportTypeId)
        {
            return await _dbSet
                .AnyAsync(c => c.Companyid == companyId && c.Reporttypeid == reportTypeId);
        }

        /// <summary>
        /// קבלת הגדרה ספציפית לחברה וסוג דיווח
        /// </summary>
        public async Task<Companyreportconfig?> GetConfigByCompanyAndReportTypeAsync(int companyId, int reportTypeId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .FirstOrDefaultAsync(c => c.Companyid == companyId && c.Reporttypeid == reportTypeId);
        }

       
        public  Task<int> CountAsync()
        {
            return  _dbSet.CountAsync();
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Companyreportconfig>> GetByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }
    }
}