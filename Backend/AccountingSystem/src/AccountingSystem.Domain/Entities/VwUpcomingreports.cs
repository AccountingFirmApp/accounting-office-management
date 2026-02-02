using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Domain.Entities
{
    public partial class VwUpcomingreports
    {
        public int? Id { get; set; }

        public string? Companyname { get; set; }

        public string? Reporttypename { get; set; }

        public string? Shortcode { get; set; }

        public DateOnly? Period { get; set; }

        public decimal? Amount { get; set; }

        public short? Dayofmonth { get; set; }

        public int? Daysoverdue { get; set; }
    }
}