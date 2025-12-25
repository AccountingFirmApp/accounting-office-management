using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("auditlog")]
[Index("Createdat", Name = "idx_audit_created")]
[Index("Entitytype", "Entityid", Name = "idx_audit_entity")]
[Index("Workerid", Name = "idx_audit_worker")]
public partial class Auditlog
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("workerid")]
    public int Workerid { get; set; }

    [Column("entitytype")]
    [StringLength(50)]
    public string Entitytype { get; set; } = null!;

    [Column("entityid")]
    public int Entityid { get; set; }

    [Column("action")]
    [StringLength(100)]
    public string Action { get; set; } = null!;

    [Column("oldvalue")]
    public string? Oldvalue { get; set; }

    [Column("newvalue")]
    public string? Newvalue { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [ForeignKey("Workerid")]
    [InverseProperty("Auditlogs")]
    public virtual Worker Worker { get; set; } = null!;
}
