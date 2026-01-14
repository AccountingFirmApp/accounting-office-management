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
    public class CompanyRepository : ICompanyRepository
    {
        private AccountingDbContext context;

        public CompanyRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public async Task<Company> AddAsync(Company entity)
        {
            await context.Companies.AddAsync(entity);
            return entity;
            Console.WriteLine("aaaaaaaaaaa");
            //throw new NotImplementedException();
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Companies.AnyAsync(cw => cw.Id == id);
        }

        public Task<IEnumerable<Company>> FindAsync(Expression<Func<Company, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetActiveCompaniesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await context.Companies.ToListAsync();
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

        System.Threading.Tasks.Task IGenericRepository<Company>.AddAsync(Company entity)
        {
            throw new NotImplementedException();
        }
    }
}
