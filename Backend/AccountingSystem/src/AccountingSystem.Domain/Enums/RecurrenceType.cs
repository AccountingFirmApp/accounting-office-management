namespace AccountingSystem.Domain.Enums
{
    /// <summary>
    /// תדירות חזרה של משימה
    /// </summary>
    public enum RecurrenceType
    {
        /// <summary>
        /// חד פעמי - לא חוזר
        /// </summary>
        OneTime=0,

        /// <summary>
        /// חודשי - כל חודש
        /// </summary>
        Monthly=1,

        /// <summary>
        /// רבעוני - כל 3 חודשים
        /// </summary>
        Quarterly=2,

        /// <summary>
        /// שנתי - פעם בשנה
        /// </summary>
        Yearly=3,

        /// <summary>
        /// דו-חודשי (חודש אחד כן חודש אחד לא)
        /// </summary>
        BiMonthly=4,

        /// <summary>
        /// מותאם אישית - לא בתבנית קבועה
        /// </summary>
        Custom=5
    }
}