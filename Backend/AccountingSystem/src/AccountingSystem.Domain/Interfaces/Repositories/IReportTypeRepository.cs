using AccountingSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IReportTypeRepository : IGenericRepository<Reporttype>
    {
        AccountingSystem.Domain.Entities.Task<Reporttype?> GetByShortcodeAsync(string shortcode);
    }
}