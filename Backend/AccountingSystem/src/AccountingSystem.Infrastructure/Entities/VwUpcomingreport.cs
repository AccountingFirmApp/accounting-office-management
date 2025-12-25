using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Keyless]
public partial class VwUpcomingreport
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("companyname")]
    [StringLength(255)]
    public string? Companyname { get; set; }

    [Column("reporttypename")]
    [StringLength(100)]
    public string? Reporttypename { get; set; }

    [Column("shortcode")]
    [StringLength(20)]
    public string? Shortcode { get; set; }

    [Column("period")]
    public DateOnly? Period { get; set; }

    [Column("amount")]
    [Precision(12, 2)]
    public decimal? Amount { get; set; }

    [Column("dayofmonth")]
    public short? Dayofmonth { get; set; }

    [Column("daysoverdue")]
    public int? Daysoverdue { get; set; }
}
