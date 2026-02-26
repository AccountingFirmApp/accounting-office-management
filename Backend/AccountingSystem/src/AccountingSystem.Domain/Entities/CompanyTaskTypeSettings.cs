using AccountingSystem.Domain.Enums;
using System;

namespace AccountingSystem.Domain.Entities

{
    /// <summary>
    /// הגדרות ספציפיות לחברה - עוקף את ברירת המחדל של TaskTypeConfiguration
    /// </summary>
    public class CompanyTaskTypeSettings
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int TaskTypeId { get; set; }

        /// <summary>
        /// האם החברה צריכה סוג משימה זה?
        /// אם false - לא תיווצר משימה אוטומטית גם אם IsMandatory=true
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// תאריך יעד מיוחד לחברה זו (עוקף את DueDayOfMonth מ-Configuration)
        /// למשל: רוב החברות מע"מ ב-15, אבל חברה זו ב-20
        /// </summary>
        public int? CustomDueDayOfMonth { get; set; }

        /// <summary>
        /// עובד קבוע שמטפל במשימה זו בחברה זו
        /// יוקצה אוטומטית בעת יצירת המשימה
        /// </summary>
        public int? DefaultAssignedWorkerId { get; set; }

        /// <summary>
        /// הערות ספציפיות לחברה - יועתקו אוטומטית לכל משימה חדשה
        /// למשל: "שימו לב - יש להם גם עסקאות במט״ח"
        /// </summary>
        public string? DefaultNotes { get; set; }

        /// <summary>
        /// עדיפות שונה לחברה זו (High/Normal/Low)
        /// </summary>
        public TaskPriority? CustomPriority { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Company Company { get; set; } = null!;
        public virtual Tasktype TaskType { get; set; } = null!;
        public virtual Worker? DefaultAssignedWorker { get; set; }
    }
}