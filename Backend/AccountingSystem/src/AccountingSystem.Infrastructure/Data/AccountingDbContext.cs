using System;
using System.Collections.Generic;
using AccountingSystem.Application.DTOs;
using AccountingSystem.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Infrastructure.Data;

public partial class AccountingDbContext : DbContext
{
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accountingfirm> Accountingfirms { get; set; }

    public virtual DbSet<Auditlog> Auditlogs { get; set; }

    public virtual DbSet<AccountingSystem.Infrastructure.Entities.Company> Companies { get; set; }

    public virtual DbSet<Companycontact> Companycontacts { get; set; }

    public virtual DbSet<Companyreportconfig> Companyreportconfigs { get; set; }

    public virtual DbSet<Companyworker> Companyworkers { get; set; }

    public virtual DbSet<Frequency> Frequencies { get; set; }

    public virtual DbSet<Reportinstance> Reportinstances { get; set; }

    public virtual DbSet<Reporttype> Reporttypes { get; set; }

    public virtual DbSet<AccountingSystem.Infrastructure.Entities.Role> Roles { get; set; }

    public virtual DbSet<AccountingSystem.Infrastructure.Entities.Task> Tasks { get; set; }

    public virtual DbSet<Tasktype> Tasktypes { get; set; }

    public virtual DbSet<VwActivetask> VwActivetasks { get; set; }

    public virtual DbSet<VwCompanydetail> VwCompanydetails { get; set; }

    public virtual DbSet<VwUpcomingreport> VwUpcomingreports { get; set; }

    public virtual DbSet<VwWorkercompany> VwWorkercompanies { get; set; }

    public virtual DbSet<AccountingSystem.Infrastructure.Entities.Worker> Workers { get; set; }

    public virtual DbSet<Workerroletype> Workerroletypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("audit_entity", new[] { "ReportInstance", "Task", "Company", "Worker" })
            .HasPostgresEnum("payment_method", new[] { "Credit", "Transfer", "Check", "Online", "Cash" })
            .HasPostgresEnum("report_status", new[] { "Pending", "Reported", "Paid", "Approved", "NotRequired" })
            .HasPostgresEnum("task_category", new[] { "Banks", "Income", "Expenses", "Reconciliations", "Other" })
            .HasPostgresEnum("task_status", new[] { "Pending", "InProgress", "Done", "Paid", "NotRequired" });

        modelBuilder.Entity<Accountingfirm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accountingfirm_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auditlog_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Worker).WithMany(p => p.Auditlogs).HasConstraintName("fk_audit_worker");
        });

        modelBuilder.Entity<AccountingSystem.Infrastructure.Entities.Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isactive).HasDefaultValue(true);
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Firm).WithMany(p => p.Companies).HasConstraintName("fk_company_firm");
        });

        modelBuilder.Entity<Companycontact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companycontact_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isprimary).HasDefaultValue(false);
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Company).WithMany(p => p.Companycontacts).HasConstraintName("fk_contact_company");
        });

        modelBuilder.Entity<Companyreportconfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companyreportconfig_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isactive).HasDefaultValue(true);
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Company).WithMany(p => p.Companyreportconfigs).HasConstraintName("fk_config_company");

            entity.HasOne(d => d.Frequency).WithMany(p => p.Companyreportconfigs)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_config_frequency");

            entity.HasOne(d => d.Reporttype).WithMany(p => p.Companyreportconfigs)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_config_reporttype");
        });

        modelBuilder.Entity<Companyworker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companyworker_pkey");

            entity.Property(e => e.Assignedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isactive).HasDefaultValue(true);

            entity.HasOne(d => d.Company).WithMany(p => p.Companyworkers).HasConstraintName("fk_companyworker_company");

            entity.HasOne(d => d.Worker).WithMany(p => p.Companyworkers).HasConstraintName("fk_companyworker_worker");
        });

        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("frequency_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Reportinstance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reportinstance_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Config).WithMany(p => p.Reportinstances).HasConstraintName("fk_instance_config");
        });

        modelBuilder.Entity<Reporttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reporttype_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<AccountingSystem.Infrastructure.Entities.Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<AccountingSystem.Infrastructure.Entities.Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Assignedworker).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_task_worker");

            entity.HasOne(d => d.Company).WithMany(p => p.Tasks).HasConstraintName("fk_task_company");

            entity.HasOne(d => d.Tasktype).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_task_tasktype");
        });

        modelBuilder.Entity<Tasktype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasktype_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Defaultorder).HasDefaultValue(99);
        });

        modelBuilder.Entity<VwActivetask>(entity =>
        {
            entity.ToView("vw_activetasks");
        });

        modelBuilder.Entity<VwCompanydetail>(entity =>
        {
            entity.ToView("vw_companydetails");
        });

        modelBuilder.Entity<VwUpcomingreport>(entity =>
        {
            entity.ToView("vw_upcomingreports");
        });

        modelBuilder.Entity<VwWorkercompany>(entity =>
        {
            entity.ToView("vw_workercompanies");
        });

        modelBuilder.Entity<AccountingSystem.Infrastructure.Entities.Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("worker_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isactive).HasDefaultValue(true);
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Firm).WithMany(p => p.Workers).HasConstraintName("fk_worker_firm");

            entity.HasOne(d => d.Role).WithMany(p => p.Workers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_worker_role");
        });

        modelBuilder.Entity<Workerroletype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workerroletype_pkey");
        });
        modelBuilder.Entity<ReportInstance>(entity =>
        {
            entity.Property(e => e.Status)
                .HasConversion<string>();

            entity.Property(e => e.PaymentMethod)
                .HasConversion<string>();
        });

        modelBuilder.Entity<AccountingSystem.Infrastructure.Entities.Task>(entity =>
        {
            entity.Property(e => e.Status)
                .HasConversion<string>();
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.Property(e => e.Category)
                .HasConversion<string>();
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
