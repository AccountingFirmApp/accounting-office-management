

using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyWorkerRepository : ICompanyWorkerRepository
    {
        private readonly AccountingDbContext _context;

        public CompanyWorkerRepository(AccountingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId)

        {
            try
            {

                var result = await _context.Companyworkers
                    .Include(cw => cw.Company)
                    .Include(cw => cw.Worker)
                    .Where(cw => cw.Workerid == workerId)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
              
                throw;
            }
        }

        public async Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Worker)
                .Include(cw => cw.Company)
                .Where(cw => cw.Companyid == companyId)
                .ToListAsync();
        }

        public async Task<bool> AssignmentExistsAsync(int companyId, int workerId)
        {
            return await _context.Companyworkers
                .AnyAsync(cw => cw.Companyid == companyId && cw.Workerid == workerId);
        }

        public async Task<Companyworker?> GetByIdAsync(int id)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)
                .Include(cw => cw.Worker)
                .FirstOrDefaultAsync(cw => cw.Id == id);
        }

        public async Task<IEnumerable<Companyworker>> GetAllAsync()
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)
                .Include(cw => cw.Worker)
                .ToListAsync();
        }

        public async Task<Companyworker> AddAsync(Companyworker entity)
        {
            await _context.Companyworkers.AddAsync(entity);
            return entity;
        }

        public async System.Threading.Tasks.Task UpdateAsync(Companyworker entity)
        {
            _context.Companyworkers.Update(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var entity = await _context.Companyworkers.FindAsync(id);
            if (entity != null)
            {
                _context.Companyworkers.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Companyworkers.AnyAsync(cw => cw.Id == id);
        }

        public async Task<IEnumerable<Companyworker>> FindAsync(Expression<Func<Companyworker, bool>> predicate)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)
                .Include(cw => cw.Worker)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<int> CountAsync(Func<object, bool> value)
        {
            return await _context.Companyworkers.CountAsync();
        }

     

    }
}
