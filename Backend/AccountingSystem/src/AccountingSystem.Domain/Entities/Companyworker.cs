using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AccountingSystem.Domain.Entities;


public partial class Companyworker
{
    public int Id { get; set; }
    public int Companyid { get; set; }
    public int Workerid { get; set; }
    public bool? Isactive { get; set; }
    public DateTime? Assignedat { get; set; }
    public virtual Company Company { get; set; } = null!;
    public virtual Worker Worker { get; set; } = null!;
}
