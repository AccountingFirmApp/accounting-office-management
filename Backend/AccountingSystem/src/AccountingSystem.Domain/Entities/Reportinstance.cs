using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingSystem.Domain.Enums;


namespace AccountingSystem.Domain.Entities;

public partial class Reportinstance
{
  
    public int Id { get; set; }
    public int Configid { get; set; }
    public DateOnly Period { get; set; }
    public decimal? Amount { get; set; }
    public ReportStatus Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public DateOnly? Receiptdate { get; set; }
    public DateOnly? Reporteddate { get; set; }
    public DateOnly? Paiddate { get; set; }
    public string? Comments { get; set; }
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
    public virtual Companyreportconfig Config { get; set; } = null!;
}
