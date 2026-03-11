using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using System.Linq.Expressions;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class AuditLogRepository :Auditlog, IAuditLogRepository
    {
        private AccountingDbContext context;

        public AuditLogRepository(AccountingDbContext context)  {
            this.context = context;
        }

        public System.Threading.Tasks.Task AddAsync(Auditlog entity)
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

        public Task<IEnumerable<Auditlog>> FindAsync(Expression<Func<Auditlog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Auditlog>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Auditlog?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, Guid entityId)
        {
            throw new NotImplementedException();

        }

        public Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(Guid workerId)
        {
            throw new NotImplementedException();

           
        }

        public Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Auditlog>> GetRecentLogsAsync(int count)
        {
            throw new NotImplementedException();

           
        }

        public System.Threading.Tasks.Task UpdateAsync(Auditlog entity)
        {
            throw new NotImplementedException();
        }
    }
}
