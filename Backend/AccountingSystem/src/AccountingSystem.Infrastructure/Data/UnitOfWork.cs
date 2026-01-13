//// AccountingSystem.Infrastructure/Data/UnitOfWork.cs

//using AccountingSystem.Domain.Interfaces;
//using AccountingSystem.Domain.Interfaces.Repositories;
//using AccountingSystem.Infrastructure.Data;
//using AccountingSystem.Infrastructure.Repositories;

//using Microsoft.EntityFrameworkCore.Storage;
//using System;
//using System.Threading.Tasks;

//namespace AccountingSystem.Infrastructure.Data
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly AccountingDbContext _context;
//        private IDbContextTransaction? _transaction;

//        // ========== כל הRepositories ==========
//        public IAccountingFirmRepository AccountingFirms { get; }
//        public ICompanyRepository Companies { get; }
//        public IWorkerRepository Workers { get; }
//        public IRoleRepository Roles { get; }
//        public ICompanyContactRepository CompanyContacts { get; }
//        public ICompanyWorkerRepository CompanyWorkers { get; }
//        public IReportTypeRepository ReportTypes { get; }
//        public IFrequencyRepository Frequencies { get; }
//        public ICompanyReportConfigRepository CompanyReportConfigs { get; }
//        public IReportInstanceRepository ReportInstances { get; }
//        public ITaskTypeRepository TaskTypes { get; }
//        public ITaskRepository Tasks { get; }
//        public IWorkerRoleTypeRepository WorkerRoleTypes { get; }
//        public IAuditLogRepository AuditLogs { get; }

//        // ========== Constructor - יוצר את כל הRepositories ==========
//        public UnitOfWork(AccountingDbContext context)
//        {
//            _context = context;

//            // צרי instance של כל repository
//            AccountingFirms = new AccountingFirmRepository(_context);
//            Companies = new CompanyRepository(_context);
//            Workers = new WorkerRepository(_context);
//            Roles = new RoleRepository(_context);
//            CompanyContacts = new CompanyContactRepository(_context);
//            CompanyWorkers = new CompanyWorkerRepository(_context);
//            ReportTypes = new ReportTypeRepository(_context);
//            Frequencies = new FrequencyRepository(_context);
//            CompanyReportConfigs = new CompanyReportConfigRepository(_context);
//            ReportInstances = new ReportInstanceRepository(_context);
//            TaskTypes = new TaskTypeRepository(_context);
//            Tasks = new TaskRepository(_context);
//            WorkerRoleTypes = new WorkerRoleTypeRepository(_context);
//            AuditLogs = new AuditLogRepository(_context);
//        }

//        // ========== שמירת שינויים ==========
//        public async Task<int> SaveChangesAsync()
//        {
//            return await _context.SaveChangesAsync();
//        }

//        // ========== ניהול טרנזקציות ==========
//        public async Task BeginTransactionAsync()
//        {
//            _transaction = await _context.Database.BeginTransactionAsync();
//        }

//        public async Task CommitTransactionAsync()
//        {
//            try
//            {
//                await SaveChangesAsync();
//                if (_transaction != null)
//                {
//                    await _transaction.CommitAsync();
//                }
//            }
//            catch
//            {
//                await RollbackTransactionAsync();
//                throw;
//            }
//            finally
//            {
//                if (_transaction != null)
//                {
//                    await _transaction.DisposeAsync();
//                    _transaction = null;
//                }
//            }
//        }

//        public async Task RollbackTransactionAsync()
//        {
//            if (_transaction != null)
//            {
//                await _transaction.RollbackAsync();
//                await _transaction.DisposeAsync();
//                _transaction = null;
//            }
//        }

//        // ========== Dispose - ניקוי משאבים ==========
//        public void Dispose()
//        {
//            _transaction?.Dispose();
//            _context.Dispose();
//        }

//        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }

//        Task IUnitOfWork.SaveChangesAsync()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
//using AccountingSystem.Domain.Interfaces.Repositories;
//using System;
//using System.Threading.Tasks;

//namespace AccountingSystem.Domain.Interfaces
//{
//    /// <summary>
//    /// ממשק UnitOfWork - מנהל את כל הRepositories ואת הטרנזקציות
//    /// מבטיח ACID: כל הפעולות מתבצעות ביחד או לא מתבצעות בכלל
//    /// </summary>
//    public interface IUnitOfWork : IDisposable
//    {
//        // ========== כל הRepositories במקום אחד ==========
//        IAccountingFirmRepository AccountingFirms { get; }
//        ICompanyRepository Companies { get; }
//        IWorkerRepository Workers { get; }
//        IRoleRepository Roles { get; }
//        ICompanyContactRepository CompanyContacts { get; }
//        ICompanyWorkerRepository CompanyWorkers { get; }
//        IReportTypeRepository ReportTypes { get; }
//        IFrequencyRepository Frequencies { get; }
//        ICompanyReportConfigRepository CompanyReportConfigs { get; }
//        IReportInstanceRepository ReportInstances { get; }
//        ITaskTypeRepository TaskTypes { get; }
//        ITaskRepository Tasks { get; }
//        IWorkerRoleTypeRepository WorkerRoleTypes { get; }
//        IAuditLogRepository AuditLogs { get; }

//        // ========== שמירת שינויים ==========
//        /// <summary>
//        /// שמור את כל השינויים לDB
//        /// מחזיר: מספר השורות שהשתנו
//        /// </summary>
//        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

//        // ========== ניהול טרנזקציות ==========
//        /// <summary>
//        /// פתח טרנזקציה חדשה
//        /// מעכשיו כל הפעולות "תלויות באוויר" עד לCommit
//        /// </summary>
//        Task BeginTransactionAsync();

//        /// <summary>
//        /// אשר את כל הפעולות - עכשיו הן באמת יישמרו!
//        /// </summary>
//        Task CommitTransactionAsync();
//        Task SaveChangesAsync();
//    }
//}   
//        /// <summary>
//        /// בטל את כל הפעולות -
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AccountingDbContext _context;

    // Lazy initialization של repositories
    private IAccountingFirmRepository? _accountingFirms;
    private ICompanyRepository? _companies;
    private IWorkerRepository? _workers;
    private IRoleRepository? _roles;
    private ICompanyContactRepository? _companyContacts;
    private ICompanyWorkerRepository? _companyWorkers;
    private IReportTypeRepository? _reportTypes;
    private IFrequencyRepository? _frequencies;
    private ICompanyReportConfigRepository? _companyReportConfigs;
    private IReportInstanceRepository? _reportInstances;
    private ITaskTypeRepository? _taskTypes;
    private ITaskRepository? _tasks;
    private IWorkerRoleTypeRepository? _workerRoleTypes;
    private IAuditLogRepository? _auditLogs;

    public UnitOfWork(AccountingDbContext context)
    {
        _context = context;
    }

    // Properties
    public IAccountingFirmRepository AccountingFirms =>
        _accountingFirms ??= new AccountingFirmRepository(_context);

    public ICompanyRepository Companies =>
        _companies ??= new CompanyRepository(_context);

    public IWorkerRepository Workers =>
        _workers ??= new WorkerRepository(_context);

    public IRoleRepository Roles =>
        _roles ??= new RoleRepository(_context);

    public ICompanyContactRepository CompanyContacts =>
        _companyContacts ??= new CompanyContactRepository(_context);

    public ICompanyWorkerRepository CompanyWorkers =>
        _companyWorkers ??= new CompanyWorkerRepository(_context);

    public IReportTypeRepository ReportTypes =>
        _reportTypes ??= new ReportTypeRepository(_context);

    public IFrequencyRepository Frequencies =>
        _frequencies ??= new FrequencyRepository(_context);

    public ICompanyReportConfigRepository CompanyReportConfigs =>
        _companyReportConfigs ??= new CompanyReportConfigRepository(_context);

    public IReportInstanceRepository ReportInstances =>
        _reportInstances ??= new ReportInstanceRepository(_context);

    public ITaskTypeRepository TaskTypes =>
        _taskTypes ??= new TaskTypeRepository(_context);

    public ITaskRepository Tasks =>
        _tasks ??= new TaskRepository(_context);

    public IWorkerRoleTypeRepository WorkerRoleTypes =>
        _workerRoleTypes ??= new WorkerRoleTypeRepository(_context);

    public IAuditLogRepository AuditLogs =>
        _auditLogs ??= new AuditLogRepository(_context);

    // ← הפונקציה החשובה!
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("💾 UnitOfWork.SaveChangesAsync נקרא");
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"✅ נשמרו {result} שורות בהצלחה");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ שגיאה בשמירה: {ex.Message}");
            Console.WriteLine($"❌ Stack: {ex.StackTrace}");
            throw;
        }
    }

    // ← הפונקציה השנייה (ללא cancellationToken)
    public async Task SaveChangesAsync()
    {
        await SaveChangesAsync(CancellationToken.None);
    }

    // Transactions
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }
    public async Task<int> UpdateTaskStatusAsync(int taskId, string status, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"💾 UnitOfWork.UpdateTaskStatusAsync - TaskId={taskId}, Status={status}");

        try
        {
            // בדוק אם הסטטוס הוא Completed
            bool isCompleted = status.Equals("Completed", StringComparison.OrdinalIgnoreCase);

            int rowsAffected;
            if (isCompleted)
            {
                rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    @"UPDATE task 
                  SET status = {0}::task_status, 
                      updatedat = NOW(),
                      completeddate = CURRENT_DATE
                  WHERE id = {1}",
                    status,
                    taskId
                );
            }
            else
            {
                rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    @"UPDATE task 
                  SET status = {0}::task_status, 
                      updatedat = NOW()
                  WHERE id = {1}",
                    status,
                    taskId
                );
            }

            Console.WriteLine($"✅ עודכנו {rowsAffected} שורות");
            return rowsAffected;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ שגיאה בעדכון סטטוס: {ex.Message}");
            Console.WriteLine($"❌ Inner: {ex.InnerException?.Message}");
            throw;
        }
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}