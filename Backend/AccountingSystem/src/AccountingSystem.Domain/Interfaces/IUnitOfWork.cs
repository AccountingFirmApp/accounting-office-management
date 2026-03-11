

using AccountingSystem.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace AccountingSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // ========== כל הRepositories במקום אחד ==========
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
        ICompanyTaskRepository Tasks { get; }
        IWorkerRoleTypeRepository WorkerRoleTypes { get; }
        IAuditLogRepository AuditLogs { get; }

        // ========== שמירת שינויים ==========
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        // ========== ניהול טרנזקציות ==========
       Task BeginTransactionAsync();
       Task CommitTransactionAsync();
        Task SaveChangesAsync();

        
    }
}