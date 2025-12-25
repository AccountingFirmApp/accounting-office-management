using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("worker")]
[Index("Email", Name = "idx_worker_email")]
[Index("Firmid", Name = "idx_worker_firm")]
[Index("Email", Name = "worker_email_key", IsUnique = true)]
public partial class Worker
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firmid")]
    public int Firmid { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string Lastname { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("employeeid")]
    [StringLength(50)]
    public string? Employeeid { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("hiredate")]
    public DateOnly? Hiredate { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [InverseProperty("Worker")]
    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();

    [InverseProperty("Worker")]
    public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();

    [ForeignKey("Firmid")]
    [InverseProperty("Workers")]
    public virtual Accountingfirm Firm { get; set; } = null!;

    [ForeignKey("Roleid")]
    [InverseProperty("Workers")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("Assignedworker")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
