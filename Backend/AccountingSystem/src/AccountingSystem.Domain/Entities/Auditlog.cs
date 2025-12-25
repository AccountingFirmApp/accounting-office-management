using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AccountingSystem.Domain.Entities;


public partial class Auditlog
{
    public long Id { get; set; }

    public int Workerid { get; set; }

    public string Entitytype { get; set; } = null!;

    public int Entityid { get; set; }

    public string Action { get; set; } = null!;

    public string? Oldvalue { get; set; }

    public string? Newvalue { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Worker Worker { get; set; } = null!;
}
