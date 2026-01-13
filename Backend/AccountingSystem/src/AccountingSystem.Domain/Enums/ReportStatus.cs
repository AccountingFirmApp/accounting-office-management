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
    Pending ,
    
    /// <summary>
    /// דווח - הדיווח בוצע אך טרם שולם
    /// </summary>
    Reported ,
    
    /// <summary>
    /// שולם - הדיווח בוצע ושולם
    /// </summary>
    Paid ,
    
    /// <summary>
    /// אושר - הדיווח אושר על ידי הרשות
    /// </summary>
    Approved ,
    
    /// <summary>
    /// לא נדרש - הדיווח לא נדרש לתקופה זו
    /// </summary>
    NotRequired 
}
