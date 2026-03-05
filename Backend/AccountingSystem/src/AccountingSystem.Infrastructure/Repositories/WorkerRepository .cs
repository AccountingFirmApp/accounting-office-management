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
    public class WorkerRepository:IWorkerRepository
    {
        private AccountingDbContext context;
        private readonly DbSet<Worker> _dbSet;

        public WorkerRepository(AccountingDbContext context)
        {
            this.context = context; 
            _dbSet = context.Workers;

        }

      
        public async Task<IEnumerable<Worker>> GetAllByFirmIdAsync(int firmId, bool? isActive = true)
        {
            var query = _dbSet
                .Include(w => w.Role)
                .Include(w => w.Firm)
                .Where(w => w.Firmid == firmId);

            if (isActive.HasValue)
                query = query.Where(w => w.Isactive == isActive.Value);

            return await query
                .OrderBy(w => w.Lastname)
                .ThenBy(w => w.Firstname)
                .ToListAsync();
        }
        public async Task<Worker?> GetByIdAndFirmIdAsync(int id, int firmId)
        {
            return await _dbSet
                .Include(w => w.Role)
                .Include(w => w.Firm)
                .FirstOrDefaultAsync(w => w.Id == id && w.Firmid == firmId);
        }

       
        public async Task<Worker?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(w => w.Role)
                .Include(w => w.Firm)
                .FirstOrDefaultAsync(w => w.Email == email);
        }
        public async Task<Worker?> GetByEmailAsync(string email, int firmId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(w => w.Email == email && w.Firmid == firmId);
        }
        public async Task AddAsync(Worker entity)
        {
            await context.AddAsync(entity);
        }

        public Task<int> CountAsync(Func<object, bool> value)

        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var worker = await context.Workers.FindAsync(id);

            if (worker == null)
                throw new KeyNotFoundException($"Worker with id {id} not found");
            worker.Isactive = false;

            context.Workers.Remove(worker);
            await context.SaveChangesAsync();
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Workers.AnyAsync(w => w.Id == id);
        }


        public Task<IEnumerable<Worker>> FindAsync(Expression<Func<Worker, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Worker>> GetActiveWorkersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Worker>> GetAllAsync()
        {
            return await _dbSet
                .Include(w => w.Role)      
                .Include(w => w.Firm)      
                .Include(w => w.Role)     
                .Include(w => w.Firm)       
                .ToListAsync();
        }
        public async Task<Worker?> GetByIdAsync(int id)
        {
            return await context.Workers
        .Include(w => w.Companyworkers.Where(cw => cw.Isactive == true))
                    .ThenInclude(wc => wc.Company)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(int companyId)
        {
            return await context.Companyworkers
                .Where(cw => cw.Companyid == companyId)
                .Include(cw => cw.Worker) 
                .Select(cw => cw.Worker)   
                .ToListAsync();
        }

        public Task<IEnumerable<Worker>> GetWorkersByFirmIdAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Worker>> GetWorkersByRoleIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<Worker?> GetWorkerWithCompaniesAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public Task<Worker?> GetWorkerWithRoleAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Worker entity)
        {
            var existingWorker = await context.Workers.FindAsync(entity.Id);
            if (existingWorker == null)
                throw new KeyNotFoundException("Worker not found."); 

            existingWorker.Firstname = entity.Firstname;
            existingWorker.Lastname = entity.Lastname;
            existingWorker.Email = entity.Email;
            existingWorker.Roleid = entity.Roleid;
            existingWorker.Firmid = entity.Firmid;
            existingWorker.Isactive = entity.Isactive;

            await context.SaveChangesAsync(); 
        }



 
    }
}
