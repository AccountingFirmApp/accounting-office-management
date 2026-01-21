using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingSystem.Domain.Enums;
namespace AccountingSystem.Domain.Entities;

public partial class Tasktype
{

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public TaskCategory Category { get; set; }
    public int? Defaultorder { get; set; }
    public DateTime? Createdat { get; set; }
    public virtual ICollection<AccountingSystem.Domain.Entities.Task> Tasks { get; set; } = new List<AccountingSystem.Domain.Entities.Task>();
}
