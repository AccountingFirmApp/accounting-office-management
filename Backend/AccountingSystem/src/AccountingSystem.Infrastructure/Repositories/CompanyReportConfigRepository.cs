using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyReportConfigRepository : ICompanyreportconfigRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<Companyreportconfig> _dbSet;

        public CompanyReportConfigRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.Companyreportconfigs;
        }


        public async Task<Companyreportconfig?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Companyreportconfig>> GetAllAsync()
        {
            var res= await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
            return res;
        }

        public async Task<IEnumerable<Companyreportconfig>> FindAsync(Expression<Func<Companyreportconfig, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                //.Include(c => c.Company)
                //.Include(c => c.Reporttype)
                //.Include(c => c.Frequency)
                .ToListAsync();
        }

        public async Task AddAsync(Companyreportconfig config)
        {
            try
            {
                

                if (config.Company != null)

                if (config.Frequency != null)

                if (config.Reporttype != null)


                _dbSet.Add(config);
                var result = await _context.SaveChangesAsync();

            }
            catch (DbUpdateException dbEx)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }


        public async Task UpdateAsync(Companyreportconfig entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }

     
        public async Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(c => c.Companyid == companyId)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .OrderBy(c => c.Reporttype.Name)
                .ToListAsync();
        }

        
        public async Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync()
        {
            return await _dbSet
                .Where(c => c.Isactive == true)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

      
        public async Task<IEnumerable<Companyreportconfig>> GetActiveConfigsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(c => c.Companyid == companyId && c.Isactive == true)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

       
        public async Task<IEnumerable<Companyreportconfig>> GetConfigsByReportTypeIdAsync(int reportTypeId)
        {
            return await _dbSet
                .Where(c => c.Reporttypeid == reportTypeId)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

       
        public async Task<IEnumerable<Companyreportconfig>> GetConfigsByFrequencyIdAsync(int frequencyId)
        {
            return await _dbSet
                .Where(c => c.Frequencyid == frequencyId)
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .ToListAsync();
        }

       
        public async Task<bool> ConfigExistsAsync(int companyId, int reportTypeId)
        {
            return await _dbSet
                .AnyAsync(c => c.Companyid == companyId && c.Reporttypeid == reportTypeId);
        }

        
        public async Task<Companyreportconfig?> GetConfigByCompanyAndReportTypeAsync(int companyId, int reportTypeId)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .FirstOrDefaultAsync(c => c.Companyid == companyId && c.Reporttypeid == reportTypeId);
        }

       
        public  Task<int> CountAsync()
        {
            return  _dbSet.CountAsync();
        }

        public Task<int> CountAsync(Func<object, bool> value)
        {
            return _dbSet.CountAsync();

        }

        public async Task<IEnumerable<Companyreportconfig>> GetByCompanyIdAsync(int companyId)
        {
            return await _dbSet
        .Where(c => c.Companyid == companyId && c.Isactive == true)
                           .Include(c => c.Company)
                           .Include(c => c.Reporttype)
                           .Include(c => c.Frequency)
                           .OrderBy(c => c.Reporttype.Name)
                           .ToListAsync();
        }

        public async Task<Companyreportconfig?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .Include(c => c.Reportinstances)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteByCompanyIdAsync(int companyId)
        {
            var configs = await _dbSet
                .Where(c => c.Companyid == companyId)
                .ToListAsync();

            _dbSet.RemoveRange(configs);
        }




        public async Task SoftDeleteByCompanyIdAsync(int companyId)
        {
            var configs = await _context.Companyreportconfigs
                .Where(c => c.Companyid == companyId)
                .ToListAsync();

            foreach (var config in configs)
                config.Isactive = false;
        }

        public async Task<List<int>> GetConfigIdsByCompanyIdAsync(int companyId)
        {
            return await _context.Companyreportconfigs
                .Where(c => c.Companyid == companyId)
                .Select(c => c.Id)
                .ToListAsync();
        }
        public async Task<List<Companyreportconfig>> GetByWorkerId(int workerId)
        {
            var companyIds = await _context.Companyworkers
                .Where(x => x.Workerid == workerId && x.Isactive == true)
                .Select(x => x.Companyid)
                .ToListAsync();

            return await _dbSet
                .Include(c => c.Company)
                .Include(c => c.Reporttype)
                .Include(c => c.Frequency)
                .Include(c => c.Reportinstances)
                .Where(c => companyIds.Contains(c.Companyid) && c.Isactive == true)
                .ToListAsync();
        }



    }

}