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

        // WorkerRepository.cs
        public async Task<IEnumerable<Worker>> GetAllAsync()
        {
            return await _dbSet
                .Include(w => w.Role)        // ✅ Worker.Role
                .Include(w => w.Firm)        // ✅ Worker.Firm (שזה Accountingfirm)
                .ToListAsync();
        }

        public Task<Worker?> GetByIdAsync(int id)
        {
            return  context.Workers
                   .FirstOrDefaultAsync(w => w.Id == id);
        }

        public Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
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
                throw new KeyNotFoundException("Worker not found."); // זה יזרוק חריגה אם לא קיים

            existingWorker.Firstname = entity.Firstname;
            existingWorker.Lastname = entity.Lastname;
            existingWorker.Email = entity.Email;
            existingWorker.Roleid = entity.Roleid;
            existingWorker.Firmid = entity.Firmid;
            existingWorker.Isactive = entity.Isactive;

            await context.SaveChangesAsync(); // ✅ בסוף המתודה יש Task שמסתיים
        }

 
    }
}
