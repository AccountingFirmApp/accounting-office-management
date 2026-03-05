using System;

namespace AccountingSystem.Domain.Entities
{
    /// <summary>
    /// המופע בפועל - הצ'קליסט של משימה ספציפית
    /// </summary>
    public class CompanyTaskChecklistItem
    {
        public int Id { get; set; }
        public int CompanyTaskId { get; set; }
        public int? TemplateItemId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? CompletedByWorkerId { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual CompanyTask CompanyTask { get; set; } = null!;
        public virtual TaskChecklistTemplateItem? TemplateItem { get; set; }
        public virtual Worker? CompletedByWorker { get; set; }
    }
}
