using AccountingSystem.Domain.Enums;
using System;

namespace AccountingSystem.Domain.Entities

{
    public class CompanyTaskTypeSettings
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int TaskTypeId { get; set; }
        public bool IsActive { get; set; }

       
        public int? CustomDueDayOfMonth { get; set; }

        public int? DefaultAssignedWorkerId { get; set; }
        public string? DefaultNotes { get; set; }
        public TaskPriority? CustomPriority { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual Company Company { get; set; } = null!;
        public virtual Tasktype TaskType { get; set; } = null!;
        public virtual Worker? DefaultAssignedWorker { get; set; }
    }
}