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
    public class CompanyWorkerRepository : ICompanyWorkerRepository
    {
        private AccountingDbContext context;

        public CompanyWorkerRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public Task<Companyworker> AddAsync(Companyworker entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AssignmentExistsAsync(int companyId, int workerId)
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

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Companyworker>> FindAsync(Expression<Func<Companyworker, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Companyworker>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Companyworker?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Companyworker entity)
        {
            throw new NotImplementedException();
        }
    }
}
