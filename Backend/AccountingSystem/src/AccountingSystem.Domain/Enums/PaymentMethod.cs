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
    Credit = 0,
    
    /// <summary>
    /// העברה בנקאית
    /// </summary>
    Transfer = 1,
    
    /// <summary>
    /// צ'ק
    /// </summary>
    Check = 2,
    
    /// <summary>
    /// תשלום מקוון (אתר הרשות)
    /// </summary>
    Online = 3,
    
    /// <summary>
    /// מזומן
    /// </summary>
    Cash = 4
}
