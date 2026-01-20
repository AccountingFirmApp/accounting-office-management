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
    public class AccountingFirmRepository:IAccountingFirmRepository
    {
        private AccountingDbContext context;

        public AccountingFirmRepository(AccountingDbContext context)
        {
            this.context = context;
        }

        public AccountingSystem.Domain.Entities.Task<Accountingfirm> AddAsync(Accountingfirm entity)
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

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Accountingfirm>> FindAsync(Expression<Func<Accountingfirm, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Accountingfirm>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetFirmWithAllDetailsAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetFirmWithCompaniesAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Accountingfirm?> GetFirmWithWorkersAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Accountingfirm entity)
        {
            throw new NotImplementedException();
        }
    }
}
