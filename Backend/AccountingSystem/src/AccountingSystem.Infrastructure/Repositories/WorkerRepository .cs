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

        public WorkerRepository(AccountingDbContext context)
        {
            this.context = context;
        }
<<<<<<< HEAD

        public AccountingSystem.Domain.Entities.Task<Worker> AddAsync(Worker entity)
=======
        public async System.Threading.Tasks.Task AddAsync(Worker entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            await context.AddAsync(entity);
        }

<<<<<<< HEAD
        public AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
=======

        public Task<int> CountAsync(Func<object, bool> value)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            throw new NotImplementedException();
        }

<<<<<<< HEAD
        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
=======
        public async System.Threading.Tasks.Task DeleteAsync(int id)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            var worker = await context.Workers.FindAsync(id);

            if (worker == null)
                throw new KeyNotFoundException($"Worker with id {id} not found");
            worker.Isactive = false;

            context.Workers.Remove(worker);
            await context.SaveChangesAsync();
        }

<<<<<<< HEAD
        public AccountingSystem.Domain.Entities.Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null)
=======

        public Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            throw new NotImplementedException();
        }


        //public async AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        //{
        //    return await context.Workers.AnyAsync(cw => cw.Id == id);
        //}


        public async AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        {
            return await context.Workers.AnyAsync(w => w.Id == id);
        }


        public AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> FindAsync(Expression<Func<Worker, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetActiveWorkersAsync()
        {
            throw new NotImplementedException();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetAllAsync()
        {
            return await context.Workers.ToListAsync();
        }

        public AccountingSystem.Domain.Entities.Task<Worker?> GetByIdAsync(int id)
        {
            return  context.Workers
                   .FirstOrDefaultAsync(w => w.Id == id);
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetWorkersByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetWorkersByFirmIdAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Worker>> GetWorkersByRoleIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Worker?> GetWorkerWithCompaniesAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Worker?> GetWorkerWithRoleAsync(int workerId)
        {
            throw new NotImplementedException();
        }

<<<<<<< HEAD
        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Worker entity)
=======
        public async System.Threading.Tasks.Task UpdateAsync(Worker entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
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
