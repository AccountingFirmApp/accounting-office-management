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
    public class CompanyRepository : ICompanyRepository
    {
        private AccountingDbContext context;

        public CompanyRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public Task<Company> AddAsync(Company entity)
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

        public Task<IEnumerable<Company>> FindAsync(Expression<Func<Company, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetActiveCompaniesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetCompaniesByFirmIdAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetCompanyWithAllDetailsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetCompanyWithContactsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetCompanyWithReportConfigsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetCompanyWithWorkersAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetInactiveCompaniesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TaxIdExistsAsync(string taxId, int? excludeCompanyId = null)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Company entity)
        {
            throw new NotImplementedException();
        }
    }
}
