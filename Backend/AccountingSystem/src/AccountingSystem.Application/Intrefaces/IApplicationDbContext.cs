//using AccountingSystem.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AccountingSystem.Application.Intrefaces
//{
//    public interface IApplicationDbContext
//    {
//        DbSet<CompanyTask> CompanyTasks { get; }
//        DbSet<Accountingfirm> Accountingfirms { get; set; }
//         DbSet<Auditlog> Auditlogs { get; set; }
//         DbSet<Company> Companies { get; set; }
//         DbSet<Companycontact> Companycontacts { get; set; }
//         DbSet<Companyreportconfig> Companyreportconfigs { get; set; }
//         DbSet<Companyworker> Companyworkers { get; set; }
//         DbSet<Frequency> Frequencies { get; set; }
//         DbSet<Reportinstance> Reportinstances { get; set; }
//        DbSet<Reporttype> Reporttypes { get; set; }
//         DbSet<Role> Roles { get; set; }
//         DbSet<Tasktype> Tasktypes { get; set; }
//        DbSet<VwActivetasks> VwActivetasks { get; set; }
//        DbSet<VwCompanydetails> VwCompanydetails { get; set; }
//        DbSet<VwUpcomingreports> VwUpcomingreports { get; set; }
//      DbSet<VwWorkercompanies> VwWorkercompanies { get; set; }
//         DbSet<Worker> Workers { get; set; }
//         DbSet<Workerroletype> Workerroletypes { get; set; }
//        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
//    }

//}
