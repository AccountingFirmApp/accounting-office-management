using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{

    public interface IReportInstanceRepository : IGenericRepository<Reportinstance>
    {

      
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByCompanyIdAsync(int companyId);

       
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByConfigIdAsync(int configId);

     
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByStatusAsync(string status);


        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetPendingReportsAsync();

      
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByPeriodAsync(DateTime period);

        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate);

      
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetOverdueReportsAsync();

       
        System.Threading.Tasks.Task<IEnumerable<Reportinstance>> GetReportsDueInNextDaysAsync(int days);
        System.Threading.Tasks.Task AddAsync(Reportinstance report);
        Task DeleteByConfigIdAsync(int configId);

        Task DeleteByConfigIdsAsync(List<int> configIds);
        public  Task<List<Reportinstance>> GenerateReportsAsync(DateTime date);
        public Task<bool> CheckReportsAsync();

    }
}