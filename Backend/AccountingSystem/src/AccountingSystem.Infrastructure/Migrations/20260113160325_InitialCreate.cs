using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .Annotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .Annotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .Annotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .Annotation("Npgsql:Enum:task_status", "Pending,InProgress,Done,Paid,NotRequired");

            migrationBuilder.CreateTable(
                name: "accountingfirm",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("accountingfirm_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "frequency",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("frequency_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reporttype",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    shortcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    officialurl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("reporttype_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tasktype",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    defaultorder = table.Column<int>(type: "integer", nullable: true, defaultValue: 99),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tasktype_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workerroletype",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("workerroletype_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firmid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    taxid = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("company_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_firm",
                        column: x => x.firmid,
                        principalTable: "accountingfirm",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "worker",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firmid = table.Column<int>(type: "integer", nullable: false),
                    roleid = table.Column<int>(type: "integer", nullable: false),
                    firstname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    employeeid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    hiredate = table.Column<DateOnly>(type: "date", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    passwordhash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    googleid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    authprovider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Local")
                },
                constraints: table =>
                {
                    table.PrimaryKey("worker_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_worker_firm",
                        column: x => x.firmid,
                        principalTable: "accountingfirm",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_worker_role",
                        column: x => x.roleid,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "companycontact",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    firstname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isprimary = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("companycontact_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_contact_company",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "companyreportconfig",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    reporttypeid = table.Column<int>(type: "integer", nullable: false),
                    frequencyid = table.Column<int>(type: "integer", nullable: false),
                    dayofmonth = table.Column<short>(type: "smallint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("companyreportconfig_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_config_company",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_config_frequency",
                        column: x => x.frequencyid,
                        principalTable: "frequency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_config_reporttype",
                        column: x => x.reporttypeid,
                        principalTable: "reporttype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "auditlog",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workerid = table.Column<int>(type: "integer", nullable: false),
                    entitytype = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    entityid = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    oldvalue = table.Column<string>(type: "text", nullable: true),
                    newvalue = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("auditlog_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_worker",
                        column: x => x.workerid,
                        principalTable: "worker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "companyworker",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    workerid = table.Column<int>(type: "integer", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    assignedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("companyworker_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_companyworker_company",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_companyworker_worker",
                        column: x => x.workerid,
                        principalTable: "worker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    tasktypeid = table.Column<int>(type: "integer", nullable: false),
                    period = table.Column<DateOnly>(type: "date", nullable: false),
                    duedate = table.Column<DateOnly>(type: "date", nullable: true),
                    completeddate = table.Column<DateOnly>(type: "date", nullable: true),
                    assignedworkerid = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_company",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_task_tasktype",
                        column: x => x.tasktypeid,
                        principalTable: "tasktype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_task_worker",
                        column: x => x.assignedworkerid,
                        principalTable: "worker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "reportinstance",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    configid = table.Column<int>(type: "integer", nullable: false),
                    period = table.Column<DateOnly>(type: "date", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    paymentmethod = table.Column<string>(type: "text", nullable: true),
                    receiptdate = table.Column<DateOnly>(type: "date", nullable: true),
                    reporteddate = table.Column<DateOnly>(type: "date", nullable: true),
                    paiddate = table.Column<DateOnly>(type: "date", nullable: true),
                    comments = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("reportinstance_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_instance_config",
                        column: x => x.configid,
                        principalTable: "companyreportconfig",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_audit_created",
                table: "auditlog",
                column: "createdat");

            migrationBuilder.CreateIndex(
                name: "idx_audit_entity",
                table: "auditlog",
                columns: new[] { "entitytype", "entityid" });

            migrationBuilder.CreateIndex(
                name: "idx_audit_worker",
                table: "auditlog",
                column: "workerid");

            migrationBuilder.CreateIndex(
                name: "idx_company_firm",
                table: "company",
                column: "firmid");

            migrationBuilder.CreateIndex(
                name: "idx_company_taxid",
                table: "company",
                column: "taxid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_contact_company",
                table: "companycontact",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "idx_config_company",
                table: "companyreportconfig",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_companyreportconfig_frequencyid",
                table: "companyreportconfig",
                column: "frequencyid");

            migrationBuilder.CreateIndex(
                name: "IX_companyreportconfig_reporttypeid",
                table: "companyreportconfig",
                column: "reporttypeid");

            migrationBuilder.CreateIndex(
                name: "uq_company_report",
                table: "companyreportconfig",
                columns: new[] { "companyid", "reporttypeid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companyworker_workerid",
                table: "companyworker",
                column: "workerid");

            migrationBuilder.CreateIndex(
                name: "uq_company_worker",
                table: "companyworker",
                columns: new[] { "companyid", "workerid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "frequency_name_key",
                table: "frequency",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_report_config",
                table: "reportinstance",
                column: "configid");

            migrationBuilder.CreateIndex(
                name: "idx_report_period",
                table: "reportinstance",
                column: "period");

            migrationBuilder.CreateIndex(
                name: "role_name_key",
                table: "role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_task_assigned",
                table: "task",
                column: "assignedworkerid");

            migrationBuilder.CreateIndex(
                name: "idx_task_company_period",
                table: "task",
                columns: new[] { "companyid", "period" });

            migrationBuilder.CreateIndex(
                name: "IX_task_tasktypeid",
                table: "task",
                column: "tasktypeid");

            migrationBuilder.CreateIndex(
                name: "uq_task_period",
                table: "task",
                columns: new[] { "companyid", "tasktypeid", "period" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_worker_firm",
                table: "worker",
                column: "firmid");

            migrationBuilder.CreateIndex(
                name: "IX_worker_roleid",
                table: "worker",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "worker_email_key",
                table: "worker",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "workerroletype_name_key",
                table: "workerroletype",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditlog");

            migrationBuilder.DropTable(
                name: "companycontact");

            migrationBuilder.DropTable(
                name: "companyworker");

            migrationBuilder.DropTable(
                name: "reportinstance");

            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "workerroletype");

            migrationBuilder.DropTable(
                name: "companyreportconfig");

            migrationBuilder.DropTable(
                name: "tasktype");

            migrationBuilder.DropTable(
                name: "worker");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropTable(
                name: "frequency");

            migrationBuilder.DropTable(
                name: "reporttype");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "accountingfirm");
        }
    }
}
