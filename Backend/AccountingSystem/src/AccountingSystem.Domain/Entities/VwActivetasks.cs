using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Domain.Entities
{
    public partial class VwActivetasks
    {
        public int? Id { get; set; }

        public string? Companyname { get; set; }

        public string? Tasktypename { get; set; }

        public DateOnly? Period { get; set; }

        public DateOnly? Duedate { get; set; }

        public string? Assignedworkername { get; set; }

        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }
    }
}
