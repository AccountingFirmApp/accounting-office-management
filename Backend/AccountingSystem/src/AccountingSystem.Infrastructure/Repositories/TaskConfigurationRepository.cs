using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskConfigurationRepository(AccountingDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbSet = context.CompanyTaskConfigurations;
            _httpContextAccessor = httpContextAccessor;
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
            return Task.CompletedTask; 
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
            // גישה ישירה לכל ה-Claims
            var claims = _httpContextAccessor.HttpContext?.User?.Claims;

            var firmIdClaim = claims?.FirstOrDefault(c => c.Type == "FirmId")?.Value;

            if (string.IsNullOrEmpty(firmIdClaim))
            {
                throw new UnauthorizedAccessException("FirmId claim missing!");
            }

            int firmId = int.Parse(firmIdClaim);
           

            return await _dbSet
                .Include(x => x.Assignedworker)
                .Include(x => x.Company)
                .Where(x => x.Company.Firmid == firmId)
                .AsNoTracking()
                .ToListAsync();
        }
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
            return await _dbSet.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}