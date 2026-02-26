using System;
using System.Collections.Generic;

namespace AccountingSystem.Domain.Entities
{
    /// <summary>
    /// תבנית צ'קליסט לסוג משימה - הרשימה הכללית של צעדים
    /// </summary>
    public class TaskChecklistTemplate
    {
        public int Id { get; set; }
        public int TaskTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool AutoCreateItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public virtual Tasktype TaskType { get; set; } = null!;
        public virtual ICollection<TaskChecklistTemplateItem> Items { get; set; }
            = new List<TaskChecklistTemplateItem>();
    }
}