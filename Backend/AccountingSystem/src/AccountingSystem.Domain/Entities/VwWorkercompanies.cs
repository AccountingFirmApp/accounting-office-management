using System;
using System.Collections.Generic;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Domain.Entities
{
    public partial class VwWorkercompanies
    {
        public int? Workerid { get; set; }

        public string? Workername { get; set; }

        public string? Workeremail { get; set; }

        public int? Companyid { get; set; }

        public string? Companyname { get; set; }

        public bool? Isactive { get; set; }

        public DateTime? Assignedat { get; set; }
    }
}
