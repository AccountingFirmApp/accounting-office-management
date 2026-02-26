using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface ICompanyreportconfigRepository : IGenericRepository<Companyreportconfig>
    {
        // פעולות בסיסיות מורחבות
        System.Threading.Tasks.Task<Companyreportconfig?> GetByIdAsync(int id);
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetAllAsync();
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task AddAsync(Companyreportconfig company);
        System.Threading.Tasks.Task<Companyreportconfig?> GetByIdWithDetailsAsync(int id);
        System.Threading.Tasks.Task<List<Companyreportconfig>?> GetByWorkerId(int workerId);


        // חיפושים ספציפיים
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetConfigsByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetActiveConfigsAsync();
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetActiveConfigsByCompanyIdAsync(int companyId);
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetConfigsByReportTypeIdAsync(int reportTypeId);
        System.Threading.Tasks.Task<IEnumerable<Companyreportconfig>> GetConfigsByFrequencyIdAsync(int frequencyId);
        
        // בדיקות
        System.Threading.Tasks.Task<bool> ConfigExistsAsync(int companyId, int reportTypeId);
        System.Threading.Tasks.Task<Companyreportconfig?> GetConfigByCompanyAndReportTypeAsync(int companyId, int reportTypeId);
        Task DeleteByCompanyIdAsync(int companyId);
        Task SoftDeleteByCompanyIdAsync(int companyId);
        Task<List<int>> GetConfigIdsByCompanyIdAsync(int companyId); // עדיין צריך את זה למחיקה הקשה
    }
}