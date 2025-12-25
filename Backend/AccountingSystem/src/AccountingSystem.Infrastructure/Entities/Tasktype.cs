using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("tasktype")]
public partial class Tasktype
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;
    [Column("category")]
    public TaskCategory Category { get; set; }
    [Column("defaultorder")]
    public int? Defaultorder { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [InverseProperty("Tasktype")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
