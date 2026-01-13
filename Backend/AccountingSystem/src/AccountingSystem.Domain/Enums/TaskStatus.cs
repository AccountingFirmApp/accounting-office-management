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
    Pending=0,

    /// <summary>
    /// בביצוע - המשימה החלה אך טרם הושלמה
    /// </summary>
    InProgress=1,

    /// <summary>
    /// בוצע - המשימה הושלמה
    /// </summary>
    Done=2,
    /// <summary>
    /// שולם - המשימה הושלמה והתשלום בוצע
    /// </summary>
    Paid=3,

    /// <summary>
    /// לא נדרש - המשימה לא נדרשת לתקופה זו
    /// </summary>
    NotRequired=4
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
