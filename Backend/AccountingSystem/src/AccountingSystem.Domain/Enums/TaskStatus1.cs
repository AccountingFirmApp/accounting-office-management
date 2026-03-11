namespace AccountingSystem.Domain.Enums;

/// <summary>
/// סטטוסים של משימות
/// תואם ל-PostgreSQL ENUM: task_status
/// </summary>
public enum TaskStatus1
{
    /// <summary>
    /// ממתין - המשימה טרם החלה
    /// </summary>
    Pending=1,
    
    /// <summary>
    /// בביצוע - המשימה החלה אך טרם הושלמה
    /// </summary>
    InProgress=2,
    
    /// <summary>
    /// בוצע - המשימה הושלמה
    /// </summary>
    Done=3,
    
    /// <summary>
    /// שולם - המשימה הושלמה והתשלום בוצע
    /// </summary>
    Paid=4,
    
    /// <summary>
    /// לא נדרש - המשימה לא נדרשת לתקופה זו
    /// </summary>
    NotRequired=5
}
