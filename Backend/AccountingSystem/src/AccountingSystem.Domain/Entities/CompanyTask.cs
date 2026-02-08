using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
namespace AccountingSystem.Domain.Entities
{
    public partial class CompanyTask
    {
        public int Id { get; set; }

        public int Companyid { get; set; }

        public int Tasktypeid { get; set; }

        public DateOnly Period { get; set; }

        public DateOnly? Duedate { get; set; }

        public DateOnly? Completeddate { get; set; }

        public int? Assignedworkerid { get; set; }

        public string? Notes { get; set; }

        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }
        public AccountingSystem.Domain.Enums.TaskStatus  ? Status { get; set; }

        public virtual Worker? Assignedworker { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual Tasktype Tasktype { get; set; } = null!;
    }
}