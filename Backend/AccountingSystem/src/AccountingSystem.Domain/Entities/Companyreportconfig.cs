using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSystem.Domain.Entities;


public partial class Companyreportconfig
{

    public int Id { get; set; }

    public int Companyid { get; set; }

    public int Reporttypeid { get; set; }

    public int Frequencyid { get; set; }

    public short? Dayofmonth { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Company Company { get; set; } = null!;
    public virtual Frequency Frequency { get; set; } = null!;
    public virtual ICollection<Reportinstance> Reportinstances { get; set; } = new List<Reportinstance>();
    public virtual Reporttype Reporttype { get; set; } = null!;
}
