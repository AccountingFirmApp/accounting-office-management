using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("company")]
[Index("Taxid", Name = "company_taxid_key", IsUnique = true)]
[Index("Firmid", Name = "idx_company_firm")]
[Index("Taxid", Name = "idx_company_taxid")]
public partial class Company
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firmid")]
    public int Firmid { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("taxid")]
    [StringLength(20)]
    public string? Taxid { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<Companycontact> Companycontacts { get; set; } = new List<Companycontact>();

    [InverseProperty("Company")]
    public virtual ICollection<Companyreportconfig> Companyreportconfigs { get; set; } = new List<Companyreportconfig>();

    [InverseProperty("Company")]
    public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();

    [ForeignKey("Firmid")]
    [InverseProperty("Companies")]
    public virtual Accountingfirm Firm { get; set; } = null!;

    [InverseProperty("Company")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
