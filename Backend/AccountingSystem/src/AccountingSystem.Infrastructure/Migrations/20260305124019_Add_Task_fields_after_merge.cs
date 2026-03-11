using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSystem.Infrastructure.Migrations
{
    public partial class Add_Task_fields_after_merge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "companytask",
                newName: "priority");
            migrationBuilder.Sql(@"
    DROP VIEW IF EXISTS vw_activetasks;

    ALTER TABLE companytask 
    ALTER COLUMN status TYPE text
    USING status::text;

    ALTER TABLE companytask ALTER COLUMN priority DROP DEFAULT;
    
    -- ממיר מספרים לשמות enum
    ALTER TABLE companytask ALTER COLUMN priority TYPE text USING priority::text;
    UPDATE companytask SET priority = CASE priority
        WHEN '0' THEN 'Low'
        WHEN '1' THEN 'Normal'
        WHEN '2' THEN 'High'
        WHEN '3' THEN 'Urgent'
        ELSE 'Normal'
    END;
    ALTER TABLE companytask 
    ALTER COLUMN priority TYPE task_priority
    USING priority::task_priority;
    ALTER TABLE companytask ALTER COLUMN priority SET DEFAULT 'Normal'::task_priority;

    CREATE VIEW vw_activetasks AS
    SELECT t.id,
        c.name AS companyname,
        tt.name AS tasktypename,
        tt.category,
        t.period,
        t.status,
        t.duedate,
        (((w.firstname)::text || ' '::text) || (w.lastname)::text) AS assignedworkername,
        t.createdat,
        t.updated_at AS updatedat
    FROM (((companytask t
        JOIN company c ON ((t.companyid = c.id)))
        JOIN tasktype tt ON ((t.tasktypeid = tt.id)))
        LEFT JOIN worker w ON ((t.assignedworkerid = w.id)))
    WHERE (t.status = ANY (ARRAY['Pending'::text, 'InProgress'::text]))
    ORDER BY t.duedate;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "priority",
                table: "companytask",
                newName: "Priority");

            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS vw_activetasks;

                ALTER TABLE companytask ALTER COLUMN priority DROP DEFAULT;
                ALTER TABLE companytask 
                ALTER COLUMN priority TYPE integer
                USING priority::text::integer;

                ALTER TABLE companytask 
                ALTER COLUMN status TYPE integer
                USING status::text::integer;

                CREATE VIEW vw_activetasks AS
                SELECT t.id,
                    c.name AS companyname,
                    tt.name AS tasktypename,
                    tt.category,
                    t.period,
                    t.status,
                    t.duedate,
                    (((w.firstname)::text || ' '::text) || (w.lastname)::text) AS assignedworkername,
                    t.createdat,
                    t.updated_at AS updatedat
                FROM (((companytask t
                    JOIN company c ON ((t.companyid = c.id)))
                    JOIN tasktype tt ON ((t.tasktypeid = tt.id)))
                    LEFT JOIN worker w ON ((t.assignedworkerid = w.id)))
                WHERE (t.status = ANY (ARRAY['Pending'::text, 'InProgress'::text]))
                ORDER BY t.duedate;
            ");
        }
    }
}