using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyContactRepository:ICompanyContactRepository
    {
        private AccountingDbContext context;

        public CompanyContactRepository(AccountingDbContext context)
        {
            this.context = context;
        }

        public AccountingSystem.Domain.Entities.Task<Companycontact> AddAsync(Companycontact entity)
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

        public AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Companycontact>> FindAsync(Expression<Func<Companycontact, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Companycontact>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Companycontact?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Companycontact>> GetContactsByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Companycontact?> GetPrimaryContactAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Companycontact entity)
        {
            throw new NotImplementedException();
        }
    }
}
