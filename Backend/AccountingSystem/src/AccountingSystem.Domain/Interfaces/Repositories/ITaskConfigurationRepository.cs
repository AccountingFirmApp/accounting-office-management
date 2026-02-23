using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;

public interface ITaskConfigurationRepository : IGenericRepository<CompanyTaskConfiguration>
{
    // מחזיר רק את השורות שקיימות בטבלה הזו
    Task<IEnumerable<CompanyTaskConfiguration>> GetAllWithWorkersAsync();
    Task<CompanyTaskConfiguration?> GetByCompanyAndTypeAsync(int companyId, int taskTypeId);


    Task AddAsync(CompanyTaskConfiguration config);
        Task UpdateAsync(CompanyTaskConfiguration config);
    Task SaveChangesAsync();
}
//}