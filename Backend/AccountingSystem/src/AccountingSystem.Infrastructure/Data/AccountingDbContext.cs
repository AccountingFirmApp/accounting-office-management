using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;

namespace AccountingSystem.Infrastructure.Data;

public partial class AccountingDbContext : DbContext
{
    static AccountingDbContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.ReportStatus>("report_status");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.PaymentMethod>("payment_method");
        NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.TaskCategory>("task_category");
    }

    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {
    }
    //public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
    //    : base(options)
    //{
    //}

    public virtual DbSet<Accountingfirm> Accountingfirms { get; set; }
    public virtual DbSet<Auditlog> Auditlogs { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<Companycontact> Companycontacts { get; set; }
    public virtual DbSet<Companyreportconfig> Companyreportconfigs { get; set; }
    public virtual DbSet<Companyworker> Companyworkers { get; set; }
    public virtual DbSet<Frequency> Frequencies { get; set; }
    public virtual DbSet<Reportinstance> Reportinstances { get; set; }
    public virtual DbSet<Reporttype> Reporttypes { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<AccountingSystem.Domain.Entities.Task> Tasks { get; set; }
    public virtual DbSet<Tasktype> Tasktypes { get; set; }
   
    public virtual DbSet<Worker> Workers { get; set; }
    public virtual DbSet<Workerroletype> Workerroletypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PostgreSQL Enums
        modelBuilder
            .HasPostgresEnum("audit_entity", new[] { "ReportInstance", "Task", "Company", "Worker" })
            .HasPostgresEnum("payment_method", new[] { "Credit", "Transfer", "Check", "Online", "Cash" })
            .HasPostgresEnum("report_status", new[] { "Pending", "Reported", "Paid", "Approved", "NotRequired" })
            .HasPostgresEnum("task_category", new[] { "Banks", "Income", "Expenses", "Reconciliations", "Other" })
            .HasPostgresEnum("task_status", new[] { "Pending", "InProgress", "Done", "Paid", "NotRequired" });

        // Accountingfirm Configuration
        modelBuilder.Entity<Accountingfirm>(entity =>
        {
            entity.ToTable("accountingfirm");

            entity.HasKey(e => e.Id).HasName("accountingfirm_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Auditlog Configuration
        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.ToTable("auditlog");

            entity.HasKey(e => e.Id).HasName("auditlog_pkey");

            entity.HasIndex(e => e.Createdat).HasDatabaseName("idx_audit_created");
            entity.HasIndex(e => new { e.Entitytype, e.Entityid }).HasDatabaseName("idx_audit_entity");
            entity.HasIndex(e => e.Workerid).HasDatabaseName("idx_audit_worker");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Workerid).HasColumnName("workerid");
            entity.Property(e => e.Entitytype)
                .HasMaxLength(50)
                .HasColumnName("entitytype");
            entity.Property(e => e.Entityid).HasColumnName("entityid");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.Oldvalue).HasColumnName("oldvalue");
            entity.Property(e => e.Newvalue).HasColumnName("newvalue");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Worker)
                .WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.Workerid)
                .HasConstraintName("fk_audit_worker");
        });

        // Company Configuration
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("company");

            entity.HasKey(e => e.Id).HasName("company_pkey");

            entity.HasIndex(e => e.Taxid).IsUnique().HasDatabaseName("company_taxid_key");
            entity.HasIndex(e => e.Firmid).HasDatabaseName("idx_company_firm");
            entity.HasIndex(e => e.Taxid).HasDatabaseName("idx_company_taxid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firmid).HasColumnName("firmid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Taxid)
                .HasMaxLength(20)
                .HasColumnName("taxid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Isactive)
                .HasColumnName("isactive")
                .HasDefaultValue(true);
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Firm)
                .WithMany(p => p.Companies)
                .HasForeignKey(d => d.Firmid)
                .HasConstraintName("fk_company_firm");
        });

        // Companycontact Configuration
        modelBuilder.Entity<Companycontact>(entity =>
        {
            entity.ToTable("companycontact");

            entity.HasKey(e => e.Id).HasName("companycontact_pkey");

            entity.HasIndex(e => e.Companyid).HasDatabaseName("idx_contact_company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");
            entity.Property(e => e.Isprimary)
                .HasColumnName("isprimary")
                .HasDefaultValue(false);
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Companycontacts)
                .HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_contact_company");
        });

        // Companyreportconfig Configuration
        modelBuilder.Entity<Companyreportconfig>(entity =>
        {
            entity.ToTable("companyreportconfig");

            entity.HasKey(e => e.Id).HasName("companyreportconfig_pkey");

            entity.HasIndex(e => e.Companyid).HasDatabaseName("idx_config_company");
            entity.HasIndex(e => new { e.Companyid, e.Reporttypeid })
                .IsUnique()
                .HasDatabaseName("uq_company_report");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Reporttypeid).HasColumnName("reporttypeid");
            entity.Property(e => e.Frequencyid).HasColumnName("frequencyid");
            entity.Property(e => e.Dayofmonth).HasColumnName("dayofmonth");
            entity.Property(e => e.Isactive)
                .HasColumnName("isactive")
                .HasDefaultValue(true);
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Companyreportconfigs)
                .HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_config_company");

            entity.HasOne(d => d.Frequency)
                .WithMany(p => p.Companyreportconfigs)
                .HasForeignKey(d => d.Frequencyid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_config_frequency");

            entity.HasOne(d => d.Reporttype)
                .WithMany(p => p.Companyreportconfigs)
                .HasForeignKey(d => d.Reporttypeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_config_reporttype");
        });

        // Companyworker Configuration
        modelBuilder.Entity<Companyworker>(entity =>
        {
            entity.ToTable("companyworker");

            entity.HasKey(e => e.Id).HasName("companyworker_pkey");

            entity.HasIndex(e => new { e.Companyid, e.Workerid }).HasDatabaseName("idx_company_worker");
            entity.HasIndex(e => new { e.Companyid, e.Workerid })
                .IsUnique()
                .HasDatabaseName("uq_company_worker");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Workerid).HasColumnName("workerid");
            entity.Property(e => e.Isactive)
                .HasColumnName("isactive")
                .HasDefaultValue(true);
            entity.Property(e => e.Assignedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Companyworkers)
                .HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_companyworker_company");

            entity.HasOne(d => d.Worker)
                .WithMany(p => p.Companyworkers)
                .HasForeignKey(d => d.Workerid)
                .HasConstraintName("fk_companyworker_worker");
        });

        // Frequency Configuration
        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.ToTable("frequency");

            entity.HasKey(e => e.Id).HasName("frequency_pkey");

            entity.HasIndex(e => e.Name).IsUnique().HasDatabaseName("frequency_name_key");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Reportinstance Configuration
        modelBuilder.Entity<Reportinstance>(entity =>
        {
            entity.ToTable("reportinstance");

            entity.HasKey(e => e.Id).HasName("reportinstance_pkey");

            entity.HasIndex(e => e.Configid).HasDatabaseName("idx_report_config");
            entity.HasIndex(e => e.Period).HasDatabaseName("idx_report_period");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Configid).HasColumnName("configid");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            
            entity.Property(e => e.Status)
      .HasColumnName("status")
    .HasColumnType("report_status");


            entity.Property(e => e.PaymentMethod)
                .HasColumnName("paymentmethod");

                //.HasConversion<string>();
            entity.Property(e => e.Receiptdate).HasColumnName("receiptdate");
            entity.Property(e => e.Reporteddate).HasColumnName("reporteddate");
            entity.Property(e => e.Paiddate).HasColumnName("paiddate");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Config)
                .WithMany(p => p.Reportinstances)
                .HasForeignKey(d => d.Configid)
                .HasConstraintName("fk_instance_config");
        });

        // Reporttype Configuration
        modelBuilder.Entity<Reporttype>(entity =>
        {
            entity.ToTable("reporttype");

            entity.HasKey(e => e.Id).HasName("reporttype_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Shortcode)
                .HasMaxLength(20)
                .HasColumnName("shortcode");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Officialurl)
                .HasMaxLength(255)
                .HasColumnName("officialurl");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Role Configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.HasIndex(e => e.Name).IsUnique().HasDatabaseName("role_name_key");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Task Configuration
        
        modelBuilder.Entity<AccountingSystem.Domain.Entities.Task>(entity =>
        {
            entity.ToTable("task");

            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.HasIndex(e => e.Assignedworkerid).HasDatabaseName("idx_task_assigned");
            entity.HasIndex(e => new { e.Companyid, e.Period }).HasDatabaseName("idx_task_company_period");
            entity.HasIndex(e => new { e.Companyid, e.Tasktypeid, e.Period })
                  .IsUnique()
                  .HasDatabaseName("uq_task_period");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Tasktypeid).HasColumnName("tasktypeid");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Completeddate).HasColumnName("completeddate");
            entity.Property(e => e.Assignedworkerid).HasColumnName("assignedworkerid");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Status)
           .HasColumnName("status")
           .HasConversion<string>()
           .HasColumnType("task_status");  // או report_status לפי השדה
         

        entity.Property(e => e.Createdat)
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("createdat")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.Updatedat)
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("updatedat")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // יחסים
            entity.HasOne(d => d.Assignedworker)
                  .WithMany(p => p.Tasks)
                  .HasForeignKey(d => d.Assignedworkerid)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("fk_task_worker");

            entity.HasOne(d => d.Company)
                  .WithMany(p => p.Tasks)
                  .HasForeignKey(d => d.Companyid)
                  .HasConstraintName("fk_task_company");

            entity.HasOne(d => d.Tasktype)
                  .WithMany(p => p.Tasks)
                  .HasForeignKey(d => d.Tasktypeid)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("fk_task_tasktype");
        });


        // Tasktype Configuration
        modelBuilder.Entity<Tasktype>(entity =>
        {
            entity.ToTable("tasktype");

            entity.HasKey(e => e.Id).HasName("tasktype_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Category)
                .HasColumnName("category")
                 .HasConversion<string>()
           .HasColumnType("task_category");
            entity.Property(e => e.Defaultorder)
                .HasColumnName("defaultorder")
                .HasDefaultValue(99);
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Worker Configuration
        modelBuilder.Entity<Worker>(entity =>
        {
            entity.ToTable("worker");

            entity.HasKey(e => e.Id).HasName("worker_pkey");

            entity.HasIndex(e => e.Email).HasDatabaseName("idx_worker_email");
            entity.HasIndex(e => e.Firmid).HasDatabaseName("idx_worker_firm");
            entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("worker_email_key");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firmid).HasColumnName("firmid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .HasColumnName("isactive")
                .HasDefaultValue(true);
            entity.Property(e => e.Hiredate).HasColumnName("hiredate");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Firm)
                .WithMany(p => p.Workers)
                .HasForeignKey(d => d.Firmid)
                .HasConstraintName("fk_worker_firm");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Workers)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_worker_role");
        });

        // Workerroletype Configuration
        modelBuilder.Entity<Workerroletype>(entity =>
        {
            entity.ToTable("workerroletype");

            entity.HasKey(e => e.Id).HasName("workerroletype_pkey");

            entity.HasIndex(e => e.Name).IsUnique().HasDatabaseName("workerroletype_name_key");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
        });

   

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
