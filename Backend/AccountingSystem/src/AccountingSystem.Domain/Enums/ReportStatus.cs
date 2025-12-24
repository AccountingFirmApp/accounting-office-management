namespace AccountingSystem.Domain.Enums;

/// <summary>
/// סטטוסים של דיווחים
/// תואם ל-PostgreSQL ENUM: report_status
/// </summary>
public enum ReportStatus
{
    /// <summary>
    /// ממתין - הדיווח עדיין לא בוצע
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// דווח - הדיווח בוצע אך טרם שולם
    /// </summary>
    Reported = 1,
    
    /// <summary>
    /// שולם - הדיווח בוצע ושולם
    /// </summary>
    Paid = 2,
    
    /// <summary>
    /// אושר - הדיווח אושר על ידי הרשות
    /// </summary>
    Approved = 3,
    
    /// <summary>
    /// לא נדרש - הדיווח לא נדרש לתקופה זו
    /// </summary>
    NotRequired = 4
}
