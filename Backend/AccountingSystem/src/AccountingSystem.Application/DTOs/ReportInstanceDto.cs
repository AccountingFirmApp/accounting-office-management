namespace AccountingSystem.Application.DTOs;

/// <summary>
/// DTO for ReportInstance - מופעי דיווחים (דיווחים שבוצעו)
/// </summary>
public class ReportInstanceDto
{
    public int Id { get; set; }
    public int ConfigId { get; set; }
    public DateTime Period { get; set; } // 2025-09-01
    public decimal? Amount { get; set; }
    public string Status { get; set; } = string.Empty; // Pending, Reported, Paid, Approved, NotRequired
    public string? PaymentMethod { get; set; } // Credit, Transfer, Check, Online, Cash
    public DateTime? ReceiptDate { get; set; } // קבלת חומר מלקוח
    public DateTime? ReportedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string Comments { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO ליצירת מופע דיווח חדש
/// </summary>
public class CreateReportInstanceDto
{
    public int ConfigId { get; set; }
    public DateTime Period { get; set; }
    public decimal? Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? ReceiptDate { get; set; }
    public string Comments { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון מופע דיווח
/// </summary>
public class UpdateReportInstanceDto
{
    public int Id { get; set; }
    public decimal? Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public DateTime? ReceiptDate { get; set; }
    public DateTime? ReportedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string Comments { get; set; } = string.Empty;
}

/// <summary>
/// DTO מורחב - עם כל הפרטים לתצוגה
/// </summary>
public class ReportInstanceDetailDto
{
    public int Id { get; set; }
    public int ConfigId { get; set; }
    
    // Company info
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyTaxId { get; set; } = string.Empty;
    
    // Report Type info
    public int ReportTypeId { get; set; }
    public string ReportTypeName { get; set; } = string.Empty;
    public string ReportTypeShortCode { get; set; } = string.Empty;
    
    // Frequency info
    public string FrequencyName { get; set; } = string.Empty;
    public short? DayOfMonth { get; set; }
    
    // Instance data
    public DateTime Period { get; set; }
    public string PeriodFormatted => Period.ToString("MM/yyyy"); // 09/2025
    public decimal? Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public DateTime? ReceiptDate { get; set; }
    public DateTime? ReportedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string Comments { get; set; } = string.Empty;
    
    // Calculated fields
    public int? DaysOverdue 
    {
        get
        {
            if (Period < DateTime.Now.Date && (Status == "Pending" || Status == "Reported"))
            {
                return (DateTime.Now.Date - Period).Days;
            }
            return null;
        }
    }
}

/// <summary>
/// DTO לעדכון סטטוס מהיר
/// </summary>
public class UpdateReportStatusDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// DTO לעדכון תשלום
/// </summary>
public class UpdateReportPaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime PaidDate { get; set; }
}

/// <summary>
/// DTO לרשימת דיווחים קרובים
/// </summary>
public class UpcomingReportDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ReportTypeName { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime Period { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public short? DayOfMonth { get; set; }
    public int DaysOverdue { get; set; }
}
