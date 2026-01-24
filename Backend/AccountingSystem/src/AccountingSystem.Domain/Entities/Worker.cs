////using AccountingSystem.Domain.Entities;
////using TaskEntity = AccountingSystem.Domain.Entities.AccountingSystem.Domain.Entities.Task;

////public partial class Worker
////{
////    public int Id { get; set; }
////    public int Firmid { get; set; }
////    public int Roleid { get; set; }
////    public string Firstname { get; set; } = null!;
////    public string Lastname { get; set; } = null!;
////    public string Email { get; set; } = null!;
////    public string? PasswordHash { get; set; }
////    public string? Phone { get; set; }
////    public string? Employeeid { get; set; }
////    public string? GoogleId { get; set; }
////    public string? AuthProvider { get; set; }
////    public bool? Isactive { get; set; }
////    public DateOnly? Hiredate { get; set; }
////    public DateTime? Createdat { get; set; }
////    public DateTime? Updatedat { get; set; }
////    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();
////    public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();
////    public virtual Accountingfirm Firm { get; set; } = null!;

////    public virtual Role Role { get; set; } = null!;

////    public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
////}


//using TaskEntity = AccountingSystem.Domain.Entities.AccountingSystem.Domain.Entities.Task;

//namespace AccountingSystem.Domain.Entities
//{
//    public partial class Worker
//    {
//        public int Id { get; set; }
//        public int Firmid { get; set; }
//        public int Roleid { get; set; }
//        public string Firstname { get; set; } = null!;
//        public string Lastname { get; set; } = null!;
//        public string Email { get; set; } = null!;
//        public string? PasswordHash { get; set; }
//        public string? Phone { get; set; }
//        public string? Employeeid { get; set; }
//        public string? GoogleId { get; set; }
//        public string? AuthProvider { get; set; }
//        public bool? Isactive { get; set; }
//        public DateOnly? Hiredate { get; set; }
//        public DateTime? Createdat { get; set; }
//        public DateTime? Updatedat { get; set; }

//        public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();
//        public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();
//        public virtual Accountingfirm Firm { get; set; } = null!;
//        public virtual Role Role { get; set; } = null!;
//        public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
//    }
//}




using System;
using System.Collections.Generic;

namespace AccountingSystem.Domain.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public int Firmid { get; set; }
        public int Roleid { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Employeeid { get; set; }
        public bool? Isactive { get; set; }
        public DateOnly? Hiredate { get; set; }
        public DateTime? Createdat { get; set; }
        public DateTime? Updatedat { get; set; }

        // 🆕 השדות החדשים שאת רוצה להוסיף:
        public string? PasswordHash { get; set; }
        public string? GoogleId { get; set; }
        public string? AuthProvider { get; set; }

        // Navigation properties
        public virtual Accountingfirm Firm { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<CompanyTask> CompanyTasks { get; set; } = new List<CompanyTask>();
        public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();
        public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();
    }
}