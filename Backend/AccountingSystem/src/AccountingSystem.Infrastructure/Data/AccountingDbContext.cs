using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TaskStatus = AccountingSystem.Domain.Enums.TaskStatus1;


namespace AccountingSystem.Infrastructure.Data;

public partial class AccountingDbContext : DbContext
{
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accountingfirm> Accountingfirms { get; set; }
    public virtual DbSet<Auditlog> Auditlogs { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<Companycontact> Companycontacts { get; set; }
    public virtual DbSet<Companyreportconfig> Companyreportconfigs { get; set; }
    public virtual DbSet<CompanyTask> CompanyTasks { get; set; }
    public virtual DbSet<Companyworker> Companyworkers { get; set; }
    public virtual DbSet<Frequency> Frequencies { get; set; }
    public virtual DbSet<Reportinstance> Reportinstances { get; set; }
    public virtual DbSet<Reporttype> Reporttypes { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Tasktype> Tasktypes { get; set; }
    public virtual DbSet<VwActivetasks> VwActivetasks { get; set; }
    public virtual DbSet<VwCompanydetails> VwCompanydetails { get; set; }
    public virtual DbSet<VwUpcomingreports> VwUpcomingreports { get; set; }
    public virtual DbSet<VwWorkercompanies> VwWorkercompanies { get; set; }
    public virtual DbSet<Worker> Workers { get; set; }
    public virtual DbSet<Workerroletype> Workerroletypes { get; set; }
    public virtual DbSet<TaskTypeConfiguration> TaskTypeConfiguration { get; set; }
    public virtual DbSet<CompanyTaskTypeSettings> CompanyTaskTypeSettings { get; set; }
    public virtual DbSet<TaskChecklistTemplate> TaskChecklistTemplate { get; set; }
    public virtual DbSet<CompanyTaskChecklistItem> CompanyTaskChecklistItems { get; set; }
    public DbSet<TaskChecklistTemplate> TaskChecklistTemplates { get; set; }
    public DbSet<TaskChecklistTemplateItem> TaskChecklistTemplateItems { get; set; }
    public DbSet<CompanyTaskConfiguration> CompanyTaskConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Tasktype>()
            .Property(e => e.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Reportinstance>()
            .Property(e => e.PaymentMethod)
            .HasConversion<string>();

        modelBuilder.Entity<Reportinstance>()
            .Property(e => e.Status)
            .HasConversion<string>();
        modelBuilder
            .HasPostgresEnum("audit_entity", new[] { "ReportInstance", "Task", "Company", "Worker" })
            .HasPostgresEnum("payment_method", new[] { "Credit", "Transfer", "Check", "Online", "Cash" })
            .HasPostgresEnum("report_status", new[] { "Pending", "Reported", "Paid", "Approved", "NotRequired" })
            .HasPostgresEnum("task_category", new[] { "Banks", "Income", "Expenses", "Reconciliations", "Other" })
            .HasPostgresEnum("TaskStatus1", new[] { "Pending", "InProgress", "Done", "Paid", "NotRequired" })
            .HasPostgresEnum("task_priority", new[] { "Low", "Normal", "High", "Urgent" })
           .HasPostgresEnum<RecurrenceType>("recurrence_type",
    nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
        modelBuilder.Entity<Accountingfirm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accountingfirm_pkey");

            entity.ToTable("accountingfirm");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auditlog_pkey");

            entity.ToTable("auditlog");

            entity.HasIndex(e => e.Createdat, "idx_audit_created");

            entity.HasIndex(e => new { e.Entitytype, e.Entityid }, "idx_audit_entity");

            entity.HasIndex(e => e.Workerid, "idx_audit_worker");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Entityid).HasColumnName("entityid");
            entity.Property(e => e.Entitytype)
                .HasMaxLength(50)
                .HasColumnName("entitytype");
            entity.Property(e => e.Newvalue).HasColumnName("newvalue");
            entity.Property(e => e.Oldvalue).HasColumnName("oldvalue");
            entity.Property(e => e.Workerid).HasColumnName("workerid");

            entity.HasOne(d => d.Worker).WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.Workerid)
                .HasConstraintName("fk_audit_worker");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_pkey");

            entity.ToTable("company");

            entity.HasIndex(e => e.Taxid, "company_taxid_key").IsUnique();

            entity.HasIndex(e => e.Firmid, "idx_company_firm");

            entity.HasIndex(e => e.Taxid, "idx_company_taxid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firmid).HasColumnName("firmid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Taxid)
                .HasMaxLength(20)
                .HasColumnName("taxid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Firm).WithMany(p => p.Companies)
                .HasForeignKey(d => d.Firmid)
                .HasConstraintName("fk_company_firm");

        });

        modelBuilder.Entity<Companycontact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companycontact_pkey");

            entity.ToTable("companycontact");

            entity.HasIndex(e => e.Companyid, "idx_contact_company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Isprimary)
                .HasDefaultValue(false)
                .HasColumnName("isprimary");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Company).WithMany(p => p.Companycontacts).HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_contact_company");
        });

        modelBuilder.Entity<Companyreportconfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companyreportconfig_pkey");

            entity.ToTable("companyreportconfig");

            entity.HasIndex(e => e.Companyid, "idx_config_company");

            entity.HasIndex(e => new { e.Companyid, e.Reporttypeid, e.Year }, "uq_company_report_year").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Dayofmonth).HasColumnName("dayofmonth");
            entity.Property(e => e.Frequencyid).HasColumnName("frequencyid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Reporttypeid).HasColumnName("reporttypeid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Year)
                .HasDefaultValueSql("EXTRACT(year FROM CURRENT_DATE)")
                .HasColumnName("year");

            entity.HasOne(d => d.Company).WithMany(p => p.Companyreportconfigs)
                .HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_config_company");

            entity.HasOne(d => d.Frequency).WithMany(p => p.Companyreportconfigs)
                .HasForeignKey(d => d.Frequencyid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_config_frequency");

            entity.HasOne(d => d.Reporttype).WithMany(p => p.Companyreportconfigs)
                .HasForeignKey(d => d.Reporttypeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_config_reporttype");
        });

        modelBuilder.Entity<CompanyTask>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("companytask");

            entity.HasIndex(e => e.Assignedworkerid, "idx_task_assigned");

            entity.HasIndex(e => new { e.Companyid, e.Period }, "idx_task_company_period");
            entity.HasIndex(e => new { e.Companyid, e.Period }, "idx_task_company_period");

            entity.HasIndex(e => new { e.Companyid, e.Tasktypeid, e.Period }, "uq_task_period").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Assignedworkerid).HasColumnName("assignedworkerid");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Completeddate).HasColumnName("completeddate");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Status)
          .HasConversion<string>()
          .HasDefaultValueSql("'Pending'::\"TaskStatus1\"")
          .HasColumnName("status");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Tasktypeid).HasColumnName("tasktypeid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Assignedworker).WithMany(p => p.CompanyTasks)
                .HasForeignKey(d => d.Assignedworkerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_task_worker");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyTasks)
                .HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_task_company");

            entity.HasOne(d => d.Tasktype).WithMany(p => p.CompanyTasks)
                .HasForeignKey(d => d.Tasktypeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_task_tasktype");
            entity.Property(e => e.Isactive)
    .HasDefaultValue(true)
    .HasColumnName("isactive");
            entity.Property(e => e.Priority)
    .HasColumnName("priority")
    .HasConversion<string>()
    .HasDefaultValueSql("'Normal'::task_priority");
        });


        modelBuilder.Entity<Companyworker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companyworker_pkey");

            entity.ToTable("companyworker");

            entity.HasIndex(e => new { e.Companyid, e.Workerid }, "idx_company_worker");

            entity.HasIndex(e => new { e.Companyid, e.Workerid }, "uq_company_worker").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Assignedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignedat");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Workerid).HasColumnName("workerid");

            entity.HasOne(d => d.Company).WithMany(p => p.Companyworkers)
                            .HasForeignKey(d => d.Companyid)
                .HasConstraintName("fk_companyworker_company");

            entity.HasOne(d => d.Worker).WithMany(p => p.Companyworkers)
                .HasForeignKey(d => d.Workerid)
                .HasConstraintName("fk_companyworker_worker");
        });

        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("frequency_pkey");

            entity.ToTable("frequency");

            entity.HasIndex(e => e.Name, "frequency_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reportinstance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reportinstance_pkey");

            entity.ToTable("reportinstance");

            entity.HasIndex(e => e.Configid, "idx_report_config");

            entity.HasIndex(e => e.Period, "idx_report_period");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.Configid).HasColumnName("configid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Paiddate).HasColumnName("paiddate");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Receiptdate).HasColumnName("receiptdate");
            entity.Property(e => e.Reporteddate).HasColumnName("reporteddate");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.Property(e => e.Status)
                .HasColumnType("report_status")
                .HasColumnName("status");

            entity.Property(e => e.PaymentMethod)
                .HasColumnType("payment_method")
                .HasColumnName("paymentmethod");

            entity.HasOne(d => d.Config).WithMany(p => p.Reportinstances)
                .HasForeignKey(d => d.Configid)
                .HasConstraintName("fk_instance_config");
        });

        modelBuilder.Entity<Reporttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reporttype_pkey");

            entity.ToTable("reporttype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Officialurl)
                .HasMaxLength(255)
                .HasColumnName("officialurl");
            entity.Property(e => e.Shortcode)
                .HasMaxLength(20)
                .HasColumnName("shortcode");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });


        // הגדרת ה-Enum עבור PostgreSQL
        modelBuilder.HasPostgresEnum<TaskCategory>();

        modelBuilder.Entity<Tasktype>(entity =>
        {
            entity.Property(e => e.Category)
                  .HasColumnType("task_category");
        });

        modelBuilder.Entity<Tasktype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasktype_pkey");

            entity.ToTable("tasktype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Category)
                .HasColumnName("category");
            entity.Property(e => e.Defaultorder)
                .HasDefaultValue(99)
                .HasColumnName("defaultorder");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VwActivetasks>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_activetasks");

            entity.Property(e => e.Assignedworkername).HasColumnName("assignedworkername");
            entity.Property(e => e.Category)
                .HasColumnName("category");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Status)
                .HasColumnName("status");
            entity.Property(e => e.Tasktypename)
                .HasMaxLength(255)
                .HasColumnName("tasktypename");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<VwCompanydetails>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_companydetails");

            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firmemail)
                .HasMaxLength(255)
                .HasColumnName("firmemail");
            entity.Property(e => e.Firmname)
                .HasMaxLength(255)
                .HasColumnName("firmname");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Taxid)
                .HasMaxLength(20)
                .HasColumnName("taxid");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<VwUpcomingreports>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_upcomingreports");

            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Dayofmonth).HasColumnName("dayofmonth");
            entity.Property(e => e.Daysoverdue).HasColumnName("daysoverdue");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Reporttypename)
                .HasMaxLength(100)
                .HasColumnName("reporttypename");
            entity.Property(e => e.Shortcode)
                .HasMaxLength(20)
                .HasColumnName("shortcode");
            entity.Property(e => e.Status)
                .HasColumnName("status");
        });

        modelBuilder.Entity<VwWorkercompanies>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_workercompanies");

            entity.Property(e => e.Assignedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignedat");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Workeremail)
                .HasMaxLength(255)
                .HasColumnName("workeremail");
            entity.Property(e => e.Workerid).HasColumnName("workerid");
            entity.Property(e => e.Workername).HasColumnName("workername");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("worker_pkey");

            entity.ToTable("worker");

            entity.HasIndex(e => e.Email, "idx_worker_email");

            entity.HasIndex(e => e.Firmid, "idx_worker_firm");

            entity.HasIndex(e => e.Email, "worker_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthProvider)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Local'::character varying")
                .HasColumnName("authprovider");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Firmid).HasColumnName("firmid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.GoogleId)
                .HasMaxLength(100)
                .HasColumnName("googleid");
            entity.Property(e => e.Hiredate).HasColumnName("hiredate");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("passwordhash");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Firm).WithMany(p => p.Workers)
                .HasForeignKey(d => d.Firmid)
                .HasConstraintName("fk_worker_firm");

            entity.HasOne(d => d.Role).WithMany(p => p.Workers)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_worker_role");
        });

        modelBuilder.Entity<Workerroletype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workerroletype_pkey");

            entity.ToTable("workerroletype");

            entity.HasIndex(e => e.Name, "workerroletype_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TaskTypeConfiguration>(entity =>
        {
            entity.ToTable("task_type_configuration");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");
            entity.Property(e => e.RecurrenceType)
    .HasColumnName("recurrence_type")
    .HasColumnType("recurrence_type");
            entity.Property(e => e.DueDayOfMonth).HasColumnName("due_day_of_month");
            entity.Property(e => e.DueDaysAfterPeriodEnd).HasColumnName("due_days_after_period_end");
            entity.Property(e => e.IsMandatory).HasColumnName("is_mandatory");
            entity.Property(e => e.AutoCreateNext).HasColumnName("auto_create_next");
            entity.Property(e => e.EstimatedHours).HasColumnName("estimated_hours");
            entity.Property(e => e.DependsOnTaskTypeId).HasColumnName("depends_on_task_type_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.TaskType).WithMany().HasForeignKey(d => d.TaskTypeId);
        });

        modelBuilder.Entity<TaskChecklistTemplate>(entity =>
        {
            entity.ToTable("task_checklist_template");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");
            entity.Property(e => e.AutoCreateItems).HasColumnName("auto_create_items");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.TaskType).WithMany().HasForeignKey(d => d.TaskTypeId);


            entity.HasMany(d => d.Items)           // לתבנית יש הרבה פריטים
              .WithOne(p => p.Template)        // לכל פריט יש תבנית אחת (המאפיין ששלחת לי במודל)
              .HasForeignKey(d => d.TemplateId)// המפתח הזר הוא TemplateId
              .HasPrincipalKey(d => d.Id);     // המפתח הראשי הוא Id
        });
        modelBuilder.Entity<TaskChecklistTemplateItem>(entity =>
        {
            entity.ToTable("task_checklist_template_item");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            // כאן אנחנו מוודאים ש-EF יודע ש-TemplateId (מהמודל) הוא העמודה template_id (ב-DB)
            entity.Property(e => e.TemplateId).HasColumnName("template_id");

            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.IsOptional).HasColumnName("is_optional");
            entity.Property(e => e.EstimatedMinutes).HasColumnName("estimated_minutes");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<CompanyTaskTypeSettings>(entity =>
        {
            entity.ToTable("company_task_type_settings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CustomDueDayOfMonth).HasColumnName("custom_due_day_of_month");
            entity.Property(e => e.DefaultAssignedWorkerId).HasColumnName("default_assigned_worker_id");
            entity.Property(e => e.DefaultNotes).HasColumnName("default_notes");

            entity.Property(e => e.CustomPriority)
                  .HasColumnName("custom_priority")
                  .HasColumnType("task_priority");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Company).WithMany().HasForeignKey(d => d.CompanyId);
            entity.HasOne(d => d.TaskType).WithMany().HasForeignKey(d => d.TaskTypeId);
        });
        modelBuilder.Entity<CompanyTaskChecklistItem>(entity =>
        {
            entity.ToTable("company_task_checklist_item");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyTaskId).HasColumnName("company_task_id");
            entity.Property(e => e.TemplateItemId).HasColumnName("template_item_id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed").HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CompletedByWorkerId).HasColumnName("completed_by_worker_id");
            entity.Property(e => e.Notes).HasColumnName("notes");

            // הגדרת הקשר למשימה (One-to-Many)
            entity.HasOne(d => d.CompanyTask)
                  .WithMany(p => p.ChecklistItems)
                  .HasForeignKey(d => d.CompanyTaskId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 2. וודאי שגם התבנית (Template) ממופה נכון כדי שה-Include יעבוד
        modelBuilder.Entity<TaskChecklistTemplate>(entity =>
        {
            entity.ToTable("task_checklist_template");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.AutoCreateItems).HasColumnName("auto_create_items");
        });

        // 3. וודאי שגם פריטי התבנית ממופים (אחרת ה-Include של ה-Items יחזור ריק)
        modelBuilder.Entity<TaskChecklistTemplateItem>(entity =>
        {
            entity.ToTable("task_checklist_template_item");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");

            entity.HasOne(d => d.Template)
                  .WithMany(p => p.Items)
                  .HasForeignKey(d => d.TemplateId);
        });

        modelBuilder.Entity<CompanyTaskConfiguration>(entity =>
        {
            // מיפוי שם הטבלה לאותיות קטנות
            entity.ToTable("companytaskconfigurations");

            entity.HasKey(e => e.Id);

            // מיפוי כל עמודה לשם המדויק ב-Postgres (אותיות קטנות)
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Companyid).HasColumnName("companyid");
            entity.Property(e => e.Tasktypeid).HasColumnName("tasktypeid");
            entity.Property(e => e.Assignedworkerid).HasColumnName("assignedworkerid");
            entity.Property(e => e.Frequency).HasColumnName("frequency");
            entity.Property(e => e.Dueday).HasColumnName("dueday");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Updatedat).HasColumnName("updatedat");

            // הגדרת קשרים (Foreign Keys) כדי שה-Include יעבוד
            entity.HasOne(d => d.Assignedworker)
                  .WithMany()
                  .HasForeignKey(d => d.Assignedworkerid)
                  .HasConstraintName("fk_config_worker");

            entity.HasOne(d => d.Company)
                  .WithMany()
                  .HasForeignKey(d => d.Companyid)
                  .HasConstraintName("fk_config_company");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
