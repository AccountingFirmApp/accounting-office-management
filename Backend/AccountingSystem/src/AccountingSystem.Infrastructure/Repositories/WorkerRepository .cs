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

        public AccountingSystem.Domain.Entities.Task<Worker> AddAsync(Worker entity)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null)
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
            throw new NotImplementedException();
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

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Worker entity)
        {
            throw new NotImplementedException();
        }
    }
}
