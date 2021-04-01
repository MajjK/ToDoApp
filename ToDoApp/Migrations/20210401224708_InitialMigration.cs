using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ToDoApp.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    password_salt = table.Column<string>(type: "text", nullable: false),
                    addition_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "date_trunc('minute'::text, CURRENT_TIMESTAMP)"),
                    role = table.Column<string>(type: "text", nullable: false, defaultValue: "user"),
                    email = table.Column<string>(type: "text", nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    objective = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    addition_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "date_trunc('minute'::text, CURRENT_TIMESTAMP)"),
                    closing_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    finished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "user_id", "addition_date", "email", "email_confirmed", "login", "password", "password_salt", "role" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@gmail.com", true, "postgres", "HcaVRCH9ST0+WePw59Qv5ghRuB1M14a/M73xT+BPxHYVtOSkZ1MG38NnYTECfBCM0duVC4+hnNEVTXVWDPxCeg==", "��K��3���I<e	0n¼��gy�A�n*(b�Q�,1�A`���Q@5l��B�1����o", "admin" },
                    { 2, new DateTime(2021, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@gmail.com", true, "postgres2", "Q9A/L2XTa9kOjCU2QnQ1Dt+YLGv0C7iqjsdoW04J+RkVuwbwr+Qy8ZweU+JTamVBy+WDxs1CBCovlqN+0rXDtw==", "3oY�-S7���Ѽ��'�A�!NɅ����Oi��8�P^}g�	�=´��H����:X�Y", "user" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "task_id", "addition_date", "closing_date", "description", "finished", "objective", "user_id" },
                values: new object[] { 1, new DateTime(2021, 3, 18, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Example Description", true, "Example Task #1 User #1", 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "task_id", "addition_date", "closing_date", "description", "objective", "user_id" },
                values: new object[] { 2, new DateTime(2021, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Example Task #2 User #1", 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "task_id", "addition_date", "closing_date", "description", "finished", "objective", "user_id" },
                values: new object[] { 3, new DateTime(2021, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Example Description", true, "Example Task #3 User #1", 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "task_id", "addition_date", "closing_date", "description", "objective", "user_id" },
                values: new object[] { 4, new DateTime(2021, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Example Task #4 User #1", 1 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "task_id", "addition_date", "closing_date", "description", "finished", "objective", "user_id" },
                values: new object[] { 5, new DateTime(2021, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Example Description", true, "Example Task #1 User #2", 2 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "task_id", "addition_date", "closing_date", "description", "objective", "user_id" },
                values: new object[] { 6, new DateTime(2021, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Example Task #2 User #2", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_user_id",
                table: "Tasks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_login",
                table: "Users",
                column: "login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
