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

        public AccountingSystem.Domain.Entities.Task<Auditlog> AddAsync(Auditlog entity)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<int> CountAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> FindAsync(Expression<Func<Auditlog, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Auditlog?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, Guid entityId)
        {
            throw new NotImplementedException();

            //return await _dbSet
            //    .Where(a => a.Entitytype == entityType && a.Entityid == entityId)
            //    .OrderByDescending(a => a.Createdat)
            //    .ToListAsync();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetLogsByEntityAsync(string entityType, int entityId)
        {
            throw new NotImplementedException();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(Guid workerId)
        {
            throw new NotImplementedException();

            //return await _dbSet
            //    .Where(a => a.Workerid == workerId)
            //    .OrderByDescending(a => a.Createdat)
            //    .ToListAsync();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetLogsByWorkerAsync(int workerId)
        {
            throw new NotImplementedException();
        }

        public async AccountingSystem.Domain.Entities.Task<IEnumerable<Auditlog>> GetRecentLogsAsync(int count)
        {
            throw new NotImplementedException();

            //return await _dbSet
            //    .OrderByDescending(a => a.Createdat)
            //    .Take(count)
            //    .Include(a => a.Worker)
            //    .ToListAsync();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Auditlog entity)
        {
            throw new NotImplementedException();
        }
    }
}
