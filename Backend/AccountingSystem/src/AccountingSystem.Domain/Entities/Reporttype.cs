using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSystem.Domain.Entities;

[Table("reporttype")]
public partial class Reporttype
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Shortcode { get; set; }
    public string? Description { get; set; }
    public string? Officialurl { get; set; }

    public DateTime? Createdat { get; set; }
    public virtual ICollection<Companyreportconfig> Companyreportconfigs { get; set; } = new List<Companyreportconfig>();
}
