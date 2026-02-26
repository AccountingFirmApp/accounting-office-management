using System;

namespace AccountingSystem.Domain.Entities
{
    /// <summary>
    /// פריט בודד בתבנית הצ'קליסט
    /// </summary>
    public class TaskChecklistTemplateItem
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsOptional { get; set; }
        public int? EstimatedMinutes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual TaskChecklistTemplate Template { get; set; } = null!;
    }
}