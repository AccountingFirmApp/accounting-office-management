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
//        AccountingSystem.Domain.Entities.Task<int> SaveChangesAsync(CancellationToken cancellationToken);

//        // ========== ניהול טרנזקציות ==========
//        /// <summary>
//        /// פתח טרנזקציה חדשה
//        /// מעכשיו כל הפעולות "תלויות באוויר" עד לCommit
//        /// </summary>
//        AccountingSystem.Domain.Entities.Task BeginTransactionAsync();

//        /// <summary>
//        /// אשר את כל הפעולות - עכשיו הן באמת יישמרו!
//        /// </summary>
//        AccountingSystem.Domain.Entities.Task CommitTransactionAsync();
//        AccountingSystem.Domain.Entities.Task SaveChangesAsync();
//    }
//}   
//        /// <summary>
//        /// בטל את כל הפעולות -
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
        ICompanyReportConfigRepository CompanyReportConfigs { get; }
        IReportInstanceRepository ReportInstances { get; }
        ITaskTypeRepository TaskTypes { get; }
        ITaskRepository Tasks { get; }
        IWorkerRoleTypeRepository WorkerRoleTypes { get; }
        IAuditLogRepository AuditLogs { get; }

        // ========== שמירת שינויים ==========
        AccountingSystem.Domain.Entities.Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        // ========== ניהול טרנזקציות ==========
        AccountingSystem.Domain.Entities.Task BeginTransactionAsync();
        AccountingSystem.Domain.Entities.Task CommitTransactionAsync();
        AccountingSystem.Domain.Entities.Task SaveChangesAsync();

        // ← הוסף את זה! פונקציה לעדכון סטטוס
        AccountingSystem.Domain.Entities.Task<int> UpdateTaskStatusAsync(int taskId, string status, CancellationToken cancellationToken = default);
    }
}