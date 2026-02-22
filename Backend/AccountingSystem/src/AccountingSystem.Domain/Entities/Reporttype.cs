using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Domain.Entities
{
    public partial class Reporttype
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Shortcode { get; set; }

        public string? Description { get; set; }

        public string? Officialurl { get; set; }

        public DateTime? Createdat { get; set; }
        public short? DefaultDayOfMonth { get; set; }

        public virtual ICollection<Companyreportconfig> Companyreportconfigs { get; set; } // ✓ רבים    }
    }
}
