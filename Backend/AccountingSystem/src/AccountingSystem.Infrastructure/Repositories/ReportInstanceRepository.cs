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
    public class ReportInstanceRepository : IReportInstanceRepository
    {
        private AccountingDbContext context;

        public ReportInstanceRepository(AccountingDbContext context)
        {
            this.context = context;
        }
        public Task<Reportinstance> AddAsync(Reportinstance entity)
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

        public Task<IEnumerable<Reportinstance>> FindAsync(Expression<Func<Reportinstance, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Reportinstance?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetPendingReportsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Reportinstance entity)
        {
            throw new NotImplementedException();
        }
    }
}
