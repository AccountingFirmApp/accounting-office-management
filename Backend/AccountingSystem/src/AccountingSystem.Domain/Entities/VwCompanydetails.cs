using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Domain.Entities
{
    public partial class VwCompanydetails
    {
        public int? Id { get; set; }

        public string? Companyname { get; set; }

        public string? Taxid { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool? Isactive { get; set; }

        public string? Firmname { get; set; }

        public string? Firmemail { get; set; }

        public DateTime? Createdat { get; set; }

        public DateTime? Updatedat { get; set; }
    }
}
