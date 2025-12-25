using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Keyless]
public partial class VwActivetask
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("companyname")]
    [StringLength(255)]
    public string? Companyname { get; set; }

    [Column("tasktypename")]
    [StringLength(255)]
    public string? Tasktypename { get; set; }

    [Column("period")]
    public DateOnly? Period { get; set; }

    [Column("duedate")]
    public DateOnly? Duedate { get; set; }

    [Column("assignedworkername")]
    public string? Assignedworkername { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }
}
