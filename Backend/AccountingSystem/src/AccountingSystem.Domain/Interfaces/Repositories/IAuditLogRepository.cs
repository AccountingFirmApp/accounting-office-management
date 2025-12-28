using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAuditLogRepository : IGenericRepository<Auditlog>
    {
        Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, Guid entityId);
        Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(Guid workerId);
        Task<IEnumerable<Auditlog>> GetRecentLogsAsync(int count);
    }
}