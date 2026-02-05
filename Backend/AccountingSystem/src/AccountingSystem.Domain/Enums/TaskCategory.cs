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
    Banks,
    
    /// <summary>
    /// משימות קשורות להכנסות (קליטת חשבוניות, הכנסות)
    /// </summary>
    Income,
    
    /// <summary>
    /// משימות קשורות להוצאות (קליטת הוצאות, חשבוניות ספקים)
    /// </summary>
    Expenses,
    
    /// <summary>
    /// משימות התאמה (התאמות בנק, התאמת קופה)
    /// </summary>
    Reconciliations,
    
    /// <summary>
    /// משימות אחרות שלא נכנסות לקטגוריות הקודמות
    /// </summary>
    Other
}
