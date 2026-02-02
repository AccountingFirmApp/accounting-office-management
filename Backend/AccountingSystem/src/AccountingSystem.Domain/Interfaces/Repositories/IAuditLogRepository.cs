using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces.Repositories
{
    public interface IAuditLogRepository : IGenericRepository<Auditlog>
    {
        System.Threading.Tasks.Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, int entityId);
        System.Threading.Tasks.Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(int workerId);
        System.Threading.Tasks.Task<IEnumerable<Auditlog>> GetRecentLogsAsync(int count);
    }
}