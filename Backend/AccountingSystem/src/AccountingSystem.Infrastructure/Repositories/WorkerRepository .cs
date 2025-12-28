using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
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

        public Task<Worker> AddAsync(Worker entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeWorkerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Worker>> FindAsync(Expression<Func<Worker, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Worker>> GetActiveWorkersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Worker>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Worker?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        public System.Threading.Tasks.Task UpdateAsync(Worker entity)
        {
            throw new NotImplementedException();
        }
    }
}
