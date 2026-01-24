using AccountingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingSystem.Domain.Entities;

public partial class Company
{
 
    public int Id { get; set; }

    public int Firmid { get; set; }

    public string Name { get; set; } = null!;

    
    public string? Taxid { get; set; }

    public string? Address { get; set; }

 
    public string? Phone { get; set; }


    public string? Email { get; set; }

    public string? Notes { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Companycontact> Companycontacts { get; set; } = new List<Companycontact>();

    public virtual ICollection<Companyreportconfig> Companyreportconfigs { get; set; } = new List<Companyreportconfig>();

    public virtual ICollection<Companyworker> Companyworkers { get; set; } = new List<Companyworker>();

    public virtual Accountingfirm Firm { get; set; } = null!;

    public virtual ICollection<CompanyTask> CompanyTasks { get; set; } = new List<CompanyTask>();
}
