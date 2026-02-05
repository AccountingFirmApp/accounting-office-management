using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixReportInstanceEnumColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropIndex(
                name: "uq_company_report",
                table: "companyreportconfig");

            migrationBuilder.RenameIndex(
                name: "idx_company_taxid",
                table: "company",
                newName: "company_taxid_key");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:TaskStatus1", "Pending,InProgress,Done,Paid,NotRequired")
                .Annotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .Annotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .Annotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .Annotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .OldAnnotation("Npgsql:Enum:audit_entity", "ReportInstance,AccountingSystem.Domain.Entities.Task,Company,Worker")
                .OldAnnotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .OldAnnotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .OldAnnotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .OldAnnotation("Npgsql:Enum:task_status", "Pending,InProgress,Done,Paid,NotRequired");

            migrationBuilder.AlterColumn<string>(
                name: "passwordhash",
                table: "worker",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValueSql: "''::character varying",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "authprovider",
                table: "worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "'Local'::character varying",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Local");

            migrationBuilder.AlterColumn<int>(
                name: "category",
                table: "tasktype",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "reportinstance",
                type: "report_status",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "paymentmethod",
                table: "reportinstance",
                type: "payment_method",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "companyreportconfig",
                type: "integer",
                nullable: false,
                defaultValueSql: "EXTRACT(year FROM CURRENT_DATE)");

            migrationBuilder.CreateTable(
                name: "companytask",
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
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "'Pending'::\"TaskStatus1\"")
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

            migrationBuilder.CreateIndex(
                name: "idx_worker_email",
                table: "worker",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_company_worker",
                table: "companyworker",
                columns: new[] { "companyid", "workerid" });

            migrationBuilder.CreateIndex(
                name: "uq_company_report_year",
                table: "companyreportconfig",
                columns: new[] { "companyid", "reporttypeid", "year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_company_taxid",
                table: "company",
                column: "taxid");

            migrationBuilder.CreateIndex(
                name: "idx_task_assigned",
                table: "companytask",
                column: "assignedworkerid");

            migrationBuilder.CreateIndex(
                name: "idx_task_company_period",
                table: "companytask",
                columns: new[] { "companyid", "period" });

            migrationBuilder.CreateIndex(
                name: "IX_companytask_tasktypeid",
                table: "companytask",
                column: "tasktypeid");

            migrationBuilder.CreateIndex(
                name: "uq_task_period",
                table: "companytask",
                columns: new[] { "companyid", "tasktypeid", "period" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companytask");

            migrationBuilder.DropIndex(
                name: "idx_worker_email",
                table: "worker");

            migrationBuilder.DropIndex(
                name: "idx_company_worker",
                table: "companyworker");

            migrationBuilder.DropIndex(
                name: "uq_company_report_year",
                table: "companyreportconfig");

            migrationBuilder.DropIndex(
                name: "idx_company_taxid",
                table: "company");

            migrationBuilder.DropColumn(
                name: "year",
                table: "companyreportconfig");

            migrationBuilder.RenameIndex(
                name: "company_taxid_key",
                table: "company",
                newName: "idx_company_taxid");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:audit_entity", "ReportInstance,AccountingSystem.Domain.Entities.Task,Company,Worker")
                .Annotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .Annotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .Annotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .Annotation("Npgsql:Enum:task_status", "Pending,InProgress,Done,Paid,NotRequired")
                .OldAnnotation("Npgsql:Enum:TaskStatus1", "Pending,InProgress,Done,Paid,NotRequired")
                .OldAnnotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .OldAnnotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .OldAnnotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .OldAnnotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other");

            migrationBuilder.AlterColumn<string>(
                name: "passwordhash",
                table: "worker",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldDefaultValueSql: "''::character varying");

            migrationBuilder.AlterColumn<string>(
                name: "authprovider",
                table: "worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Local",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "'Local'::character varying");

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "tasktype",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "reportinstance",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "report_status",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "paymentmethod",
                table: "reportinstance",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "payment_method",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assignedworkerid = table.Column<int>(type: "integer", nullable: true),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    tasktypeid = table.Column<int>(type: "integer", nullable: false),
                    completeddate = table.Column<DateOnly>(type: "date", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    duedate = table.Column<DateOnly>(type: "date", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    period = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "uq_company_report",
                table: "companyreportconfig",
                columns: new[] { "companyid", "reporttypeid" },
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
        }
    }
}
