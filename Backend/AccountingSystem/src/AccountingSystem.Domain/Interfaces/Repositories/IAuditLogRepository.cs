using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAuditLogRepository : IGenericRepository<Auditlog>
    {
        Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, int entityId);
        Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(int workerId);
        Task<IEnumerable<Auditlog>> GetRecentLogsAsync(int count);
    }
}