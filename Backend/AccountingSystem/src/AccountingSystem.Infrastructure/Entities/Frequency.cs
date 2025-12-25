using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("frequency")]
[Index("Name", Name = "frequency_name_key", IsUnique = true)]
public partial class Frequency
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [InverseProperty("Frequency")]
    public virtual ICollection<Companyreportconfig> Companyreportconfigs { get; set; } = new List<Companyreportconfig>();
}
