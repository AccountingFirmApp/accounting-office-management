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
        //private readonly DbSet<Domain.Entities.Companyreportconfig> _dbSet;

        public CompanyReportConfigRepository(AccountingDbContext context)
        {
            this.context = context;
        }

<<<<<<< HEAD
        public AccountingSystem.Domain.Entities.Task<Companyreportconfig> AddAsync(Companyreportconfig entity)
=======
        //Task<Companyreportconfig> IGenericRepository<Companyreportconfig>.AddAsync(Companyreportconfig entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public System.Threading.Tasks.Task AddAsync(Companyreportconfig entity)
        //{
        //    await context.AddAsync(entity);

        //    //throw new NotImplementedException();
        //}


        //public async Task<Domain.Entities.Companyreportconfig> AddAsync(Domain.Entities.Companyreportconfig entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    return entity;
        //}


        //public async Task AddAsync(Companyreportconfig entity)
        //{
        //    await context.Companyreportconfigs.AddAsync(entity);
        //    await context.SaveChangesAsync();
        //}
        //System.Threading.Tasks.Task IGenericRepository<Companyreportconfig>.AddAsync(Companyreportconfig entity)
        //{
        //    return AddAsync(entity);
        //}

        //public async Task<Companyreportconfig> AddAsync(Companyreportconfig entity)
        //{
        //    await context.Companyreportconfigs.AddAsync(entity);
        //    return entity;
        //}
        //public async Task AddAsync(Companyreportconfig entity)
        //{
        //    await context.Companyreportconfigs.AddAsync(entity);
        //    await context.SaveChangesAsync();
        //}
        //System.Threading.Tasks.Task IGenericRepository<Companyreportconfig>.AddAsync(Companyreportconfig entity)
        //{
        //    return AddAsync(entity);
        //}

        //public async Task<Companyreportconfig> AddAsync(Companyreportconfig entity)
        //{
        //    await context.Companyreportconfigs.AddAsync(entity);
        //    await context.SaveChangesAsync();  // ✅ חובה!
        //    return entity;
        //}
        //public Task<int> CountAsync(Func<object, bool> value)
        //{
        //    throw new NotImplementedException();
        //}


        System.Threading.Tasks.Task IGenericRepository<Companyreportconfig>.AddAsync(Companyreportconfig entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            return AddAsync(entity);
        }

<<<<<<< HEAD
        public AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
=======
        public async Task<Companyreportconfig> AddAsync(Companyreportconfig entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
        {
            await context.Companyreportconfigs.AddAsync(entity);
            // ❌ אל תשמרי כאן!
            // await context.SaveChangesAsync();  
            return entity;
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

        public Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }







        // ... שאר המתודות
    }
    }

