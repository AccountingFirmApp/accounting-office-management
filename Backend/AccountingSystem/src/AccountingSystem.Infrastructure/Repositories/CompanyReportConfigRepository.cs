using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyReportConfigRepository:ICompanyReportConfigRepository
    {
        private AccountingDbContext context;

        public CompanyReportConfigRepository(AccountingDbContext context)
        {
            this.context = context;
        }

        public AccountingSystem.Domain.Entities.Task<Companyreportconfig> AddAsync(Companyreportconfig entity)
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

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> FindAsync(Expression<Func<Companyreportconfig, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync()
        {
            throw new NotImplementedException();
        }

        //public AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public AccountingSystem.Domain.Entities.Task<Companyreportconfig?> GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Companyreportconfig entity)
        {
            throw new NotImplementedException();
        }



      

            public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetAllAsync()
            {
                return await context.Companyreportconfigs
                    .Include(c => c.Company)
                    .Include(c => c.Reporttype)
                    .Include(c => c.Frequency)
                    .ToListAsync();
            }

            public async AccountingSystem.Domain.Entities.Task<Companyreportconfig?> GetByIdAsync(int id)
            {
                return await context.Companyreportconfigs
                    .Include(c => c.Company)
                    .Include(c => c.Reporttype)
                    .Include(c => c.Frequency)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            // 👇 הוסיפי את זה
            public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyreportconfig>> GetByCompanyIdAsync(int companyId)
            {
                return await context.Companyreportconfigs
                    .Include(c => c.Company)
                    .Include(c => c.Reporttype)
                    .Include(c => c.Frequency)
                    .Where(c => c.Companyid == companyId)
                    .ToListAsync();
            }

            // ... שאר המתודות
        }
    }

