using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("companyreportconfig")]
[Index("Companyid", Name = "idx_config_company")]
[Index("Companyid", "Reporttypeid", Name = "uq_company_report", IsUnique = true)]
public partial class Companyreportconfig
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("companyid")]
    public int Companyid { get; set; }

    [Column("reporttypeid")]
    public int Reporttypeid { get; set; }

    [Column("frequencyid")]
    public int Frequencyid { get; set; }

    [Column("dayofmonth")]
    public short? Dayofmonth { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Companyid")]
    [InverseProperty("Companyreportconfigs")]
    public virtual Company Company { get; set; } = null!;

    [ForeignKey("Frequencyid")]
    [InverseProperty("Companyreportconfigs")]
    public virtual Frequency Frequency { get; set; } = null!;

    [InverseProperty("Config")]
    public virtual ICollection<Reportinstance> Reportinstances { get; set; } = new List<Reportinstance>();

    [ForeignKey("Reporttypeid")]
    [InverseProperty("Companyreportconfigs")]
    public virtual Reporttype Reporttype { get; set; } = null!;
}
