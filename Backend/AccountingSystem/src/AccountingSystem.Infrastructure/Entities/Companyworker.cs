using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("companyworker")]
[Index("Companyid", "Workerid", Name = "idx_company_worker")]
[Index("Companyid", "Workerid", Name = "uq_company_worker", IsUnique = true)]
public partial class Companyworker
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("companyid")]
    public int Companyid { get; set; }

    [Column("workerid")]
    public int Workerid { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("assignedat", TypeName = "timestamp without time zone")]
    public DateTime? Assignedat { get; set; }

    [ForeignKey("Companyid")]
    [InverseProperty("Companyworkers")]
    public virtual Company Company { get; set; } = null!;

    [ForeignKey("Workerid")]
    [InverseProperty("Companyworkers")]
    public virtual Worker Worker { get; set; } = null!;
}
