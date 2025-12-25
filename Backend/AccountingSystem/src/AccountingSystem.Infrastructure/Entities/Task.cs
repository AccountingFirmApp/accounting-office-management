using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("task")]
[Index("Assignedworkerid", Name = "idx_task_assigned")]
[Index("Companyid", "Period", Name = "idx_task_company_period")]
[Index("Companyid", "Tasktypeid", "Period", Name = "uq_task_period", IsUnique = true)]
public partial class Task
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("companyid")]
    public int Companyid { get; set; }

    [Column("tasktypeid")]
    public int Tasktypeid { get; set; }

    [Column("period")]
    public DateOnly Period { get; set; }

    [Column("duedate")]
    public DateOnly? Duedate { get; set; }

    [Column("completeddate")]
    public DateOnly? Completeddate { get; set; }

    [Column("assignedworkerid")]
    public int? Assignedworkerid { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("status")]
    public TaskStatus Status { get; set; }
    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Assignedworkerid")]
    [InverseProperty("Tasks")]
    public virtual Worker? Assignedworker { get; set; }

    [ForeignKey("Companyid")]
    [InverseProperty("Tasks")]
    public virtual Company Company { get; set; } = null!;

    [ForeignKey("Tasktypeid")]
    [InverseProperty("Tasks")]
    public virtual Tasktype Tasktype { get; set; } = null!;
}
