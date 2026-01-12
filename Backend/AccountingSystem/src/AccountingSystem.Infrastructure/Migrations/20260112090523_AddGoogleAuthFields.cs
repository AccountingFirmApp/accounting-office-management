using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleAuthFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "authprovider",
                table: "worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Local");

            migrationBuilder.AddColumn<string>(
                name: "googleid",
                table: "worker",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authprovider",
                table: "worker");

            migrationBuilder.DropColumn(
                name: "googleid",
                table: "worker");

          
        }
    }
}
