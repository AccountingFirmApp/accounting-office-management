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
    public class TaskConfigurationRepository : ITaskConfigurationRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<CompanyTaskConfiguration> _dbSet;

        public TaskConfigurationRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.CompanyTaskConfigurations;
        }

        // ==================== פעולות בסיסיות (כדי לממש את IGenericRepository) ====================

        public async Task<CompanyTaskConfiguration?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<CompanyTaskConfiguration>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(CompanyTaskConfiguration entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(CompanyTaskConfiguration entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask; // מחזירים משימה שהושלמה כי אין כאן פעולת IO אסינכרונית
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
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        // ==================== פעולות ייחודיות למטריצה ====================

        public async Task<IEnumerable<CompanyTaskConfiguration>> GetAllWithWorkersAsync()
        {
            return await _dbSet
                .Include(x => x.Assignedworker)
                .AsNoTracking()
                .ToListAsync();
        }

        // פונקציה לבדיקה אם הגדרה קיימת (לפי לקוח וסוג משימה)
        public async Task<CompanyTaskConfiguration?> GetByCompanyAndTypeAsync(int companyId, int taskTypeId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Companyid == companyId && x.Tasktypeid == taskTypeId);
        }
        public async Task<IEnumerable<CompanyTaskConfiguration>> FindAsync(Expression<Func<CompanyTaskConfiguration, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<int> CountAsync(Func<object, bool> predicate)
        {
            // מימוש תואם למה שכתבת ב-CompanyRepository
            return await _dbSet.CountAsync();
        }

        // בתוך ה-Repository Implementation
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}