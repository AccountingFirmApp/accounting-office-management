using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSystem.Domain.Entities;
public partial class Role
{

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Createdat { get; set; }
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
