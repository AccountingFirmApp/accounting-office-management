using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    public partial class AddDefaultDayOfMonthProper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // הוספת העמודה אם אינה קיימת
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_name = 'reporttype'
        AND column_name = 'defaultdayofmonth'
    ) THEN
        ALTER TABLE reporttype
        ADD COLUMN ""DefaultDayOfMonth"" smallint NULL;
    END IF;
END$$;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // הסרת העמודה רק אם קיימת
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_name = 'reporttype'
        AND column_name = 'defaultdayofmonth'
    ) THEN
        ALTER TABLE reporttype
        DROP COLUMN ""DefaultDayOfMonth"";
    END IF;
END$$;
");
        }
    }
}
