using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;

namespace AccountingSystem.Domain.Entities
{
    public partial class Tasktype
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int? Defaultorder { get; set; }
        public TaskCategory Category { get; set; }

        public DateTime? Createdat { get; set; }

        public virtual ICollection<CompanyTask> CompanyTasks { get; set; } = new List<CompanyTask>();
    }
}
