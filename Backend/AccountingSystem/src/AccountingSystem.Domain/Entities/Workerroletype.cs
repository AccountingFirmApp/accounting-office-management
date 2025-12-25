using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSystem.Domain.Entities;


public partial class Workerroletype
{
  
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
