using System;
using AccountingSystem.Domain.Enums;

namespace AccountingSystem.Domain.Entities
{
    /// <summary>
    /// הגדרות תצורה לסוג משימה - קובע איך המשימה מתנהגת
    /// </summary>
    public class TaskTypeConfiguration
    {
        public int Id { get; set; }

        public int TaskTypeId { get; set; }

        /// <summary>
        /// כל כמה זמן המשימה חוזרת
        /// </summary>
        public RecurrenceType RecurrenceType { get; set; }

        /// <summary>
        /// באיזה יום בחודש תאריך היעד (למשל: 15 = ה-15 בכל חודש)
        /// משמש למשימות חודשיות/רבעוניות
        /// </summary>
        public int? DueDayOfMonth { get; set; }

        /// <summary>
        /// כמה ימים אחרי סוף התקופה תאריך היעד
        /// למשל: 15 = 15 ימים אחרי סוף החודש
        /// אלטרנטיבה ל-DueDayOfMonth
        /// </summary>
        public int? DueDaysAfterPeriodEnd { get; set; }

        /// <summary>
        /// האם המשימה חובה לכל החברות?
        /// true = תיווצר אוטומטית לכולם
        /// false = רק אם החברה סימנה שהיא צריכה את זה
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// האם ליצור אוטומטית את המשימה הבאה כשמסיימים את הנוכחית?
        /// </summary>
        public bool AutoCreateNext { get; set; }

        /// <summary>
        /// כמה שעות בערך לוקח לבצע את המשימה (לתכנון עומסים)
        /// </summary>
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// האם המשימה תלויה במשימה אחרת?
        /// למשל: "דיווח מע״מ" תלוי ב"סגירת חודש"
        /// </summary>
        public int? DependsOnTaskTypeId { get; set; }

        /// <summary>
        /// האם התצורה פעילה (אפשר להשבית בלי למחוק)
        /// </summary>
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Tasktype TaskType { get; set; } = null!;
        public virtual Tasktype? DependsOnTaskType { get; set; }
    }
}