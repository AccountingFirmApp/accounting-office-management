using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class RoleRepository:IRoleRepository
    {
        private AccountingDbContext context;

        public RoleRepository(AccountingDbContext context)
        {
            this.context = context;
        }

<<<<<<< HEAD
        public AccountingSystem.Domain.Entities.Task<Role> AddAsync(Role entity)
=======
        public System.Threading.Tasks.Task AddAsync(Role entity)
>>>>>>> 3a3e52f6f454f8a1f7839d1e39a03267125b0a43
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

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<IEnumerable<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Role?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public AccountingSystem.Domain.Entities.Task<Role?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.AccountingSystem.Domain.Entities.Task UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
