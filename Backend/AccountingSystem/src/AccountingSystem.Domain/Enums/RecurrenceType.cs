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
        OneTime,

        /// <summary>
        /// חודשי - כל חודש
        /// </summary>
        Monthly,

        /// <summary>
        /// רבעוני - כל 3 חודשים
        /// </summary>
        Quarterly,

        /// <summary>
        /// שנתי - פעם בשנה
        /// </summary>
        Yearly,

        /// <summary>
        /// דו-חודשי (חודש אחד כן חודש אחד לא)
        /// </summary>
        BiMonthly,

        /// <summary>
        /// מותאם אישית - לא בתבנית קבועה
        /// </summary>
        Custom
    }
}