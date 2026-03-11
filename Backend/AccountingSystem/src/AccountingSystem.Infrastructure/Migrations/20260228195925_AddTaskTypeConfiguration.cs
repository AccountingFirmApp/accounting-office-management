using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskTypeConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updatedat",
                table: "companytask",
                newName: "updated_at");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:TaskStatus1", "Pending,InProgress,Done,Paid,NotRequired")
                .Annotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .Annotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .Annotation("Npgsql:Enum:recurrence_type", "Daily,Weekly,Monthly,Quarterly,Yearly")
                .Annotation("Npgsql:Enum:recurrence_type.recurrence_type", "one_time,monthly,quarterly,yearly,bi_monthly,custom")
                .Annotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .Annotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .Annotation("Npgsql:Enum:task_priority", "Low,Normal,High,Urgent")
                .OldAnnotation("Npgsql:Enum:TaskStatus1", "Pending,InProgress,Done,Paid,NotRequired")
                .OldAnnotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .OldAnnotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .OldAnnotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .OldAnnotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "companytask",
                type: "integer",
                nullable: true,
                defaultValueSql: "'Pending'::\"TaskStatus1\"",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValueSql: "'Pending'::\"TaskStatus1\"");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "companytask",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "companytask",
                type: "boolean",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isactive",
                table: "company",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                name: "company_task_type_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    task_type_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    custom_due_day_of_month = table.Column<int>(type: "integer", nullable: true),
                    default_assigned_worker_id = table.Column<int>(type: "integer", nullable: true),
                    default_notes = table.Column<string>(type: "text", nullable: true),
                    custom_priority = table.Column<int>(type: "task_priority", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_task_type_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_company_task_type_settings_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_task_type_settings_tasktype_task_type_id",
                        column: x => x.task_type_id,
                        principalTable: "tasktype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_task_type_settings_worker_default_assigned_worker_id",
                        column: x => x.default_assigned_worker_id,
                        principalTable: "worker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "companytaskconfigurations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    companyid = table.Column<int>(type: "integer", nullable: false),
                    tasktypeid = table.Column<int>(type: "integer", nullable: false),
                    assignedworkerid = table.Column<int>(type: "integer", nullable: true),
                    frequency = table.Column<int>(type: "integer", nullable: false),
                    dueday = table.Column<int>(type: "integer", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companytaskconfigurations", x => x.id);
                    table.ForeignKey(
                        name: "FK_companytaskconfigurations_tasktype_tasktypeid",
                        column: x => x.tasktypeid,
                        principalTable: "tasktype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_config_company",
                        column: x => x.companyid,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_config_worker",
                        column: x => x.assignedworkerid,
                        principalTable: "worker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "task_checklist_template",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    task_type_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    auto_create_items = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_checklist_template", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_checklist_template_tasktype_task_type_id",
                        column: x => x.task_type_id,
                        principalTable: "tasktype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_type_configuration",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    task_type_id = table.Column<int>(type: "integer", nullable: false),
                    recurrence_type = table.Column<int>(type: "recurrence_type", nullable: false),
                    due_day_of_month = table.Column<int>(type: "integer", nullable: true),
                    due_days_after_period_end = table.Column<int>(type: "integer", nullable: true),
                    is_mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    auto_create_next = table.Column<bool>(type: "boolean", nullable: false),
                    estimated_hours = table.Column<int>(type: "integer", nullable: true),
                    depends_on_task_type_id = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_type_configuration", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_type_configuration_tasktype_depends_on_task_type_id",
                        column: x => x.depends_on_task_type_id,
                        principalTable: "tasktype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_task_type_configuration_tasktype_task_type_id",
                        column: x => x.task_type_id,
                        principalTable: "tasktype",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_checklist_template_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    template_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    is_optional = table.Column<bool>(type: "boolean", nullable: false),
                    estimated_minutes = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_checklist_template_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_checklist_template_item_task_checklist_template_templa~",
                        column: x => x.template_id,
                        principalTable: "task_checklist_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_task_checklist_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_task_id = table.Column<int>(type: "integer", nullable: false),
                    template_item_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_by_worker_id = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_task_checklist_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_company_task_checklist_item_companytask_company_task_id",
                        column: x => x.company_task_id,
                        principalTable: "companytask",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_company_task_checklist_item_task_checklist_template_item_te~",
                        column: x => x.template_item_id,
                        principalTable: "task_checklist_template_item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_company_task_checklist_item_worker_completed_by_worker_id",
                        column: x => x.completed_by_worker_id,
                        principalTable: "worker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_company_task_checklist_item_company_task_id",
                table: "company_task_checklist_item",
                column: "company_task_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_task_checklist_item_completed_by_worker_id",
                table: "company_task_checklist_item",
                column: "completed_by_worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_task_checklist_item_template_item_id",
                table: "company_task_checklist_item",
                column: "template_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_task_type_settings_company_id",
                table: "company_task_type_settings",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_task_type_settings_default_assigned_worker_id",
                table: "company_task_type_settings",
                column: "default_assigned_worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_task_type_settings_task_type_id",
                table: "company_task_type_settings",
                column: "task_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_companytaskconfigurations_assignedworkerid",
                table: "companytaskconfigurations",
                column: "assignedworkerid");

            migrationBuilder.CreateIndex(
                name: "IX_companytaskconfigurations_companyid",
                table: "companytaskconfigurations",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_companytaskconfigurations_tasktypeid",
                table: "companytaskconfigurations",
                column: "tasktypeid");

            migrationBuilder.CreateIndex(
                name: "IX_task_checklist_template_task_type_id",
                table: "task_checklist_template",
                column: "task_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_checklist_template_item_template_id",
                table: "task_checklist_template_item",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_type_configuration_depends_on_task_type_id",
                table: "task_type_configuration",
                column: "depends_on_task_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_type_configuration_task_type_id",
                table: "task_type_configuration",
                column: "task_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_config_company",
                table: "companyreportconfig");

            migrationBuilder.DropTable(
                name: "company_task_checklist_item");

            migrationBuilder.DropTable(
                name: "company_task_type_settings");

            migrationBuilder.DropTable(
                name: "companytaskconfigurations");

            migrationBuilder.DropTable(
                name: "task_type_configuration");

            migrationBuilder.DropTable(
                name: "task_checklist_template_item");

            migrationBuilder.DropTable(
                name: "task_checklist_template");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "companytask");

            migrationBuilder.DropColumn(
                name: "isactive",
                table: "companytask");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "companytask",
                newName: "updatedat");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:TaskStatus1", "Pending,InProgress,Done,Paid,NotRequired")
                .Annotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .Annotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .Annotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .Annotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .OldAnnotation("Npgsql:Enum:TaskStatus1", "Pending,InProgress,Done,Paid,NotRequired")
                .OldAnnotation("Npgsql:Enum:audit_entity", "ReportInstance,Task,Company,Worker")
                .OldAnnotation("Npgsql:Enum:payment_method", "Credit,Transfer,Check,Online,Cash")
                .OldAnnotation("Npgsql:Enum:recurrence_type", "Daily,Weekly,Monthly,Quarterly,Yearly")
                .OldAnnotation("Npgsql:Enum:recurrence_type.recurrence_type", "one_time,monthly,quarterly,yearly,bi_monthly,custom")
                .OldAnnotation("Npgsql:Enum:report_status", "Pending,Reported,Paid,Approved,NotRequired")
                .OldAnnotation("Npgsql:Enum:task_category", "Banks,Income,Expenses,Reconciliations,Other")
                .OldAnnotation("Npgsql:Enum:task_priority", "Low,Normal,High,Urgent");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "companytask",
                type: "text",
                nullable: true,
                defaultValueSql: "'Pending'::\"TaskStatus1\"",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValueSql: "'Pending'::\"TaskStatus1\"");

            migrationBuilder.AlterColumn<bool>(
                name: "isactive",
                table: "company",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }
    }
}
