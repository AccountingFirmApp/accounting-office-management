//using NpgsqlTypes;

namespace AccountingSystem.Domain.Enums;

/// <summary>
/// סטטוסים של משימות
/// תואם ל-PostgreSQL ENUM: task_status
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// ממתין - המשימה טרם החלה
    /// </summary>
    Pending ,
    
    /// <summary>
    /// בביצוע - המשימה החלה אך טרם הושלמה
    /// </summary>
    InProgress ,
    
    /// <summary>
    /// בוצע - המשימה הושלמה
    /// </summary>
    Done ,
    
    /// <summary>
    /// שולם - המשימה הושלמה והתשלום בוצע
    /// </summary>
    Paid ,
    
    /// <summary>
    /// לא נדרש - המשימה לא נדרשת לתקופה זו
    /// </summary>
    NotRequired 
}

//public enum TaskStatus
//{
//    [PgName("Pending")]
//    Pending,

//    [PgName("InProgress")]
//    InProgress,

//    [PgName("Done")]
//    Done,

//    [PgName("Paid")]
//    Paid,

//    [PgName("NotRequired")]
//    NotRequired
//}
