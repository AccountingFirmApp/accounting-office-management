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
    public class AccountingFirmRepository : IAccountingFirmRepository
    {
        private AccountingDbContext context;
        private readonly DbSet<Accountingfirm> _dbSet;

        public AccountingFirmRepository(AccountingDbContext context)
        {
            this.context = context;
            _dbSet = context.Accountingfirms;

        }

        public async System.Threading.Tasks.Task AddAsync(Accountingfirm entity)
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

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(f => f.Id == id);
        }

        public Task<IEnumerable<Accountingfirm>> FindAsync(Expression<Func<Accountingfirm, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Accountingfirm>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public Task<Accountingfirm?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Accountingfirm?> GetFirmWithAllDetailsAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public Task<Accountingfirm?> GetFirmWithCompaniesAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public Task<Accountingfirm?> GetFirmWithWorkersAsync(int firmId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Accountingfirm entity)
        {
            throw new NotImplementedException();
        }
    }
    }


//        public async Task<PagedResult<Accountingfirm>> GetPageAsync(int page, int pageSize)
//        {
//            var total = await _dbSet.CountAsync();
//            var items = await _dbSet
//            .Skip((page - 1) * pageSize)
//            .Take(pageSize)
//            .ToListAsync();


//            return new PagedResult<Accountingfirm>
//            {
//                Items = items,
//                TotalCount = total
//            };
//        }
//    }
//}