namespace AccountingSystem.Domain.Enums;

/// <summary>
/// אמצעי תשלום
/// תואם ל-PostgreSQL ENUM: payment_method
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// כרטיס אשראי
    /// </summary>
    Credit,
    
    /// <summary>
    /// העברה בנקאית
    /// </summary>
    Transfer,
    
    /// <summary>
    /// צ'ק
    /// </summary>
    Check,
    
    /// <summary>
    /// תשלום מקוון (אתר הרשות)
    /// </summary>
    Online,
    
    /// <summary>
    /// מזומן
    /// </summary>
    Cash
}
