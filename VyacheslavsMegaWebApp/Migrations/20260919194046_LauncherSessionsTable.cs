using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class LauncherSessionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LauncherSessions",
                columns: table => new
                {
                    SessionKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LauncherSessions", x => x.SessionKey);
                    table.ForeignKey(
                        name: "FK_LauncherSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75ea60cd-f90f-45a2-8b5a-2ba3372fd500", "AQAAAAIAAYagAAAAELNLeQYidM0O9aNtQ/QU/Sh0qMgrZqbtcWa97UpitLn3TaKnVXaqcHoybfoUiBarpw==", "69c467b4-c352-4271-a50b-f78a3cf2b980" });

            migrationBuilder.CreateIndex(
                name: "IX_LauncherSessions_UserId",
                table: "LauncherSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LauncherSessions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7e29b993-73c5-4a32-8cdd-e8dc0c7892ca", "AQAAAAIAAYagAAAAEPRGyNKDSt55tJNbfSuvdpIaHPcUzOARUsPC/Ra9tRDxPf2hdHkz+COm81Jp00iUcA==", "d48502c4-c8e3-4535-be3e-673c4dd5d5a7" });
        }
    }
}
