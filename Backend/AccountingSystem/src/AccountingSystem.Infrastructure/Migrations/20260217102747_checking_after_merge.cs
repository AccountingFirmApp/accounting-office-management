using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class checking_after_merge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "category",
                table: "tasktype",
                type: "task_category",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "category",
                table: "tasktype",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "task_category");

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
        }
    }
}
