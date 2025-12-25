using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSystem.Domain.Entities;


public partial class Worker
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
    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();
    public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();
    public virtual Accountingfirm Firm { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
