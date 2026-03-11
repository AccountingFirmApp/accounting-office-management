using AccountingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    

    namespace AccountingSystem.Domain.Entities
    {
    [Table("companytaskconfigurations")]
    public partial class CompanyTaskConfiguration
        {
            public int Id { get; set; }

            public int Companyid { get; set; }

            public int Tasktypeid { get; set; }

            public int? Assignedworkerid { get; set; }

        [Column("frequency")] 
        public RecurrenceType Frequency { get; set; } = RecurrenceType.Monthly;
        public int Dueday { get; set; }

            public bool Isactive { get; set; }

            public DateTime? Createdat { get; set; }

            public DateTime? Updatedat { get; set; }

            // קשרי גומלין 
            public virtual Company Company { get; set; } = null!;
            public virtual Tasktype Tasktype { get; set; } = null!;
            public virtual Worker? Assignedworker { get; set; }
        }
    }