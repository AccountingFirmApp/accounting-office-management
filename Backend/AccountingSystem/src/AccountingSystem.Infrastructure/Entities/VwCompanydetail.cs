using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Keyless]
public partial class VwCompanydetail
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("companyname")]
    [StringLength(255)]
    public string? Companyname { get; set; }

    [Column("taxid")]
    [StringLength(20)]
    public string? Taxid { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("firmname")]
    [StringLength(255)]
    public string? Firmname { get; set; }

    [Column("firmemail")]
    [StringLength(255)]
    public string? Firmemail { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }
}
