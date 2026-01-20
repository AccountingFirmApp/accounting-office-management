//using AccountingSystem.Domain.Entities;
//using AccountingSystem.Domain.Interfaces.Repositories;
//using AccountingSystem.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace AccountingSystem.Infrastructure.Repositories
//{
//    public class CompanyWorkerRepository : ICompanyWorkerRepository
//    {
//        private AccountingDbContext _context;

//        public CompanyWorkerRepository(AccountingDbContext context)
//        {
//            this._context = context;
//        }


//            // ⭐ המתודה החשובה ביותר
//            public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId)
//            {
//                return await _context.Companyworkers
//                    .Include(cw => cw.Company)   // טוען את פרטי החברה
//                    .Include(cw => cw.Worker)    // טוען את פרטי העובדת
//                    .Where(cw => cw.Workerid == workerId)
//                    .ToListAsync();
//            }

//            public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId)
//            {
//                return await _context.Companyworkers
//                    .Include(cw => cw.Worker)
//                    .Include(cw => cw.Company)
//                    .Where(cw => cw.Companyid == companyId)
//                    .ToListAsync();
//            }

//            public async AccountingSystem.Domain.Entities.Task<bool> AssignmentExistsAsync(int companyId, int workerId)
//            {
//                return await _context.Companyworkers
//                    .AnyAsync(cw => cw.Companyid == companyId && cw.Workerid == workerId);
//            }

//            public async AccountingSystem.Domain.Entities.Task<Companyworker?> GetByIdAsync(int id)
//            {
//                return await _context.Companyworkers
//                    .Include(cw => cw.Company)
//                    .Include(cw => cw.Worker)
//                    .FirstOrDefaultAsync(cw => cw.Id == id);
//            }

//            public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetAllAsync()
//            {
//                return await _context.Companyworkers
//                    .Include(cw => cw.Company)
//                    .Include(cw => cw.Worker)
//                    .ToListAsync();
//            }

//            public async AccountingSystem.Domain.Entities.Task<Companyworker> AddAsync(Companyworker entity)
//            {
//                await _context.Companyworkers.AddAsync(entity);
//                return entity;
//            }

//            public async System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Companyworker entity)
//            {
//                _context.Companyworkers.Update(entity);
//                await System.Threading.Tasks.AccountingSystem.Domain.Entities.Task.CompletedTask;
//            }

//            public async System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
//            {
//                var entity = await _context.Companyworkers.FindAsync(id);
//                if (entity != null)
//                {
//                    _context.Companyworkers.Remove(entity);
//                }
//            }

//            public async AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
//            {
//                return await _context.Companyworkers.AnyAsync(cw => cw.Id == id);
//            }

//            public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> FindAsync(Expression<Func<Companyworker, bool>> predicate)
//            {
//                return await _context.Companyworkers
//                    .Include(cw => cw.Company)
//                    .Include(cw => cw.Worker)
//                    .Where(predicate)
//                    .ToListAsync();
//            }

//            public async AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
//            {
//                return await _context.Companyworkers.CountAsync();
//            }
//        }
//    }





using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyWorkerRepository : ICompanyWorkerRepository
    {
        private readonly AccountingDbContext _context;

        public CompanyWorkerRepository(AccountingDbContext context)
        {
            _context = context;
        }

        // ⭐ המתודה החשובה ביותר
        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetByWorkerIdAsync(int workerId)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)   // טוען את פרטי החברה
                .Include(cw => cw.Worker)    // טוען את פרטי העובדת
                .Where(cw => cw.Workerid == workerId)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Worker)
                .Include(cw => cw.Company)
                .Where(cw => cw.Companyid == companyId)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<bool> AssignmentExistsAsync(int companyId, int workerId)
        {
            return await _context.Companyworkers
                .AnyAsync(cw => cw.Companyid == companyId && cw.Workerid == workerId);
        }

        public async AccountingSystem.Domain.Entities.Task<Companyworker?> GetByIdAsync(int id)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)
                .Include(cw => cw.Worker)
                .FirstOrDefaultAsync(cw => cw.Id == id);
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> GetAllAsync()
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)
                .Include(cw => cw.Worker)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<Companyworker> AddAsync(Companyworker entity)
        {
            await _context.Companyworkers.AddAsync(entity);
            return entity;
        }

        public async System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Companyworker entity)
        {
            _context.Companyworkers.Update(entity);
            await System.Threading.Tasks.AccountingSystem.Domain.Entities.Task.CompletedTask;
        }

        public async System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
        {
            var entity = await _context.Companyworkers.FindAsync(id);
            if (entity != null)
            {
                _context.Companyworkers.Remove(entity);
            }
        }

        public async AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        {
            return await _context.Companyworkers.AnyAsync(cw => cw.Id == id);
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Companyworker>> FindAsync(Expression<Func<Companyworker, bool>> predicate)
        {
            return await _context.Companyworkers
                .Include(cw => cw.Company)
                .Include(cw => cw.Worker)
                .Where(predicate)
                .ToListAsync();
        }

        public async AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
        {
            return await _context.Companyworkers.CountAsync();
        }
    }
}
