using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Entities;

[Table("reportinstance")]
[Index("Configid", Name = "idx_report_config")]
[Index("Period", Name = "idx_report_period")]
public partial class Reportinstance
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("configid")]
    public int Configid { get; set; }

    [Column("period")]
    public DateOnly Period { get; set; }

    [Column("amount")]
    [Precision(12, 2)]
    public decimal? Amount { get; set; }
    [Column("status")]
    public ReportStatus Status { get; set; }

    [Column("paymentmethod")]
    public PaymentMethod? PaymentMethod { get; set; }
    [Column("receiptdate")]
    public DateOnly? Receiptdate { get; set; }

    [Column("reporteddate")]
    public DateOnly? Reporteddate { get; set; }

    [Column("paiddate")]
    public DateOnly? Paiddate { get; set; }

    [Column("comments")]
    public string? Comments { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Configid")]
    [InverseProperty("Reportinstances")]
    public virtual Companyreportconfig Config { get; set; } = null!;
}
