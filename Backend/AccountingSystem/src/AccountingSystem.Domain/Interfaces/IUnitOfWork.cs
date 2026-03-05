using AccountingSystem.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountingFirmRepository AccountingFirms { get; }
        ICompanyRepository Companies { get; }
        IWorkerRepository Workers { get; }
        IRoleRepository Roles { get; }
        ICompanyContactRepository CompanyContacts { get; }
        ICompanyWorkerRepository CompanyWorkers { get; }
        IReportTypeRepository ReportTypes { get; }
        IFrequencyRepository Frequencies { get; }
        ICompanyreportconfigRepository CompanyReportConfigs { get; }  
        IReportInstanceRepository ReportInstances { get; }
        ITaskTypeRepository TaskTypes { get; }
        ICompanyTaskRepository CompanyTasks { get; } 

        IWorkerRoleTypeRepository WorkerRoleTypes { get; }
        IAuditLogRepository AuditLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        // ========== ניהול טרנזקציות ==========
       Task BeginTransactionAsync();
       Task CommitTransactionAsync();
        Task SaveChangesAsync();
       Task<int> UpdateTaskStatusAsync(int taskId, string status, CancellationToken cancellationToken = default);
    }
}