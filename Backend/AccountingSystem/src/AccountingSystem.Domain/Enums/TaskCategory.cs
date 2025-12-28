namespace AccountingSystem.Domain.Enums;

/// <summary>
/// קטגוריות משימות
/// תואם ל-PostgreSQL ENUM: task_category
/// </summary>
public enum TaskCategory
{
    /// <summary>
    /// משימות קשורות לבנקים (קליטת דפי בנק, התאמות בנק)
    /// </summary>
    Banks = 0,
    
    /// <summary>
    /// משימות קשורות להכנסות (קליטת חשבוניות, הכנסות)
    /// </summary>
    Income = 1,
    
    /// <summary>
    /// משימות קשורות להוצאות (קליטת הוצאות, חשבוניות ספקים)
    /// </summary>
    Expenses = 2,
    
    /// <summary>
    /// משימות התאמה (התאמות בנק, התאמת קופה)
    /// </summary>
    Reconciliations = 3,
    
    /// <summary>
    /// משימות אחרות שלא נכנסות לקטגוריות הקודמות
    /// </summary>
    Other = 4
}
