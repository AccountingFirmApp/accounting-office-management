// AccountingSystem.Infrastructure/Data/UnitOfWork.cs

using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccountingDbContext _context;
        private IDbContextTransaction? _transaction;

        // ========== כל הRepositories ==========
        public IAccountingFirmRepository AccountingFirms { get; }
        public ICompanyRepository Companies { get; }
        public IWorkerRepository Workers { get; }
        public IRoleRepository Roles { get; }
        public ICompanyContactRepository CompanyContacts { get; }
        public ICompanyWorkerRepository CompanyWorkers { get; }
        public IReportTypeRepository ReportTypes { get; }
        public IFrequencyRepository Frequencies { get; }
        public ICompanyReportConfigRepository CompanyReportConfigs { get; }
        public IReportInstanceRepository ReportInstances { get; }
        public ITaskTypeRepository TaskTypes { get; }
        public ITaskRepository Tasks { get; }
        public IWorkerRoleTypeRepository WorkerRoleTypes { get; }
        public IAuditLogRepository AuditLogs { get; }

        // ========== Constructor - יוצר את כל הRepositories ==========
        public UnitOfWork(AccountingDbContext context)
        {
            _context = context;

            // צרי instance של כל repository
            AccountingFirms = new AccountingFirmRepository(_context);
            Companies = new CompanyRepository(_context);
            Workers = new WorkerRepository(_context);
            Roles = new RoleRepository(_context);
            CompanyContacts = new CompanyContactRepository(_context);
            CompanyWorkers = new CompanyWorkerRepository(_context);
            ReportTypes = new ReportTypeRepository(_context);
            Frequencies = new FrequencyRepository(_context);
            CompanyReportConfigs = new CompanyReportConfigRepository(_context);
            ReportInstances = new ReportInstanceRepository(_context);
            TaskTypes = new TaskTypeRepository(_context);
            Tasks = new TaskRepository(_context);
            WorkerRoleTypes = new WorkerRoleTypeRepository(_context);
            AuditLogs = new AuditLogRepository(_context);
        }

        // ========== שמירת שינויים ==========
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // ========== ניהול טרנזקציות ==========
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // ========== Dispose - ניקוי משאבים ==========
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IUnitOfWork.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}