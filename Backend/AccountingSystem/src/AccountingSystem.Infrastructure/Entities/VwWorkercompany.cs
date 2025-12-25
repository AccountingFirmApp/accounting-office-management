using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Keyless]
public partial class VwWorkercompany
{
    [Column("workerid")]
    public int? Workerid { get; set; }

    [Column("workername")]
    public string? Workername { get; set; }

    [Column("workeremail")]
    [StringLength(255)]
    public string? Workeremail { get; set; }

    [Column("companyid")]
    public int? Companyid { get; set; }

    [Column("companyname")]
    [StringLength(255)]
    public string? Companyname { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("assignedat", TypeName = "timestamp without time zone")]
    public DateTime? Assignedat { get; set; }
}
