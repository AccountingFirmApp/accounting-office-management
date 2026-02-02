using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Domain.Entities
{
    public partial class Accountingfirm
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }

        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

        public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
    }
}
