using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAuditLogRepository : IGenericRepository<Auditlog>
    {
        AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, int entityId);
        AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(int workerId);
        AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetRecentLogsAsync(int count);
    }
}