using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    private ICompanyreportconfigRepository? _companyReportConfigs;
    private IReportInstanceRepository? _reportInstances;
    private ITaskTypeRepository? _taskTypes;
    private ICompanyTaskRepository? _tasks;  // ?? שונה מ-ITaskRepository!
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

    public ICompanyreportconfigRepository CompanyReportConfigs =>
        _companyReportConfigs ??= new CompanyReportConfigRepository(_context);

    public IReportInstanceRepository ReportInstances =>
        _reportInstances ??= new ReportInstanceRepository(_context);

    public ITaskTypeRepository TaskTypes =>
        _taskTypes ??= new TaskTypeRepository(_context);

    public ICompanyTaskRepository Tasks =>
        _tasks ??= new CompanyTaskRepository(_context);  // ?? שונה מ-TaskRepository!

    public IWorkerRoleTypeRepository WorkerRoleTypes =>
        _workerRoleTypes ??= new WorkerRoleTypeRepository(_context);

    public IAuditLogRepository AuditLogs =>
        _auditLogs ??= new AuditLogRepository(_context);

    // ? הפונקציה החשובה!
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("?? UnitOfWork.SaveChangesAsync נקרא");
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"? נשמרו {result} שורות בהצלחה");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? שגיאה בשמירה: {ex.Message}");
            Console.WriteLine($"? Stack: {ex.StackTrace}");
            throw;
        }
    }

    // ? הפונקציה השנייה (ללא cancellationToken)
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
        Console.WriteLine($"?? UnitOfWork.UpdateTaskStatusAsync - TaskId={taskId}, Status={status}");

        try
        {
            // בדוק אם הסטטוס הוא Completed
            bool isCompleted = status.Equals("Completed", StringComparison.OrdinalIgnoreCase);

            int rowsAffected;
            if (isCompleted)
            {
                rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    @"UPDATE companytask 
                  SET status = {0}::task_status, 
                      updatedat = NOW(),
                      completeddate = CURRENT_DATE
                  WHERE id = {1}",
                    status,
                    taskId,
                    cancellationToken
                );
            }
            else
            {
                rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    @"UPDATE companytask 
                  SET status = {0}::task_status, 
                      updatedat = NOW()
                  WHERE id = {1}",
                    status,
                    taskId,
                    cancellationToken
                );
            }

            Console.WriteLine($"? עודכנו {rowsAffected} שורות");
            return rowsAffected;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? שגיאה בעדכון סטטוס: {ex.Message}");
            Console.WriteLine($"? Inner: {ex.InnerException?.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}