using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ReportText = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReports", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f21f2ff4-f4a7-40c3-92d8-a5b186b20f1f", "AQAAAAIAAYagAAAAEIR4aROJ3sRDyBKf/HFPMG8xk7z76cPbQtgZDo4gAePbXzMF9VJMNYyVpO91cvP2KA==", "361172ba-1677-44ef-9454-27b989ed0285" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReports");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3deafef9-3642-40d3-ba74-d503bf685727", "AQAAAAIAAYagAAAAEJ7uu0QmjMPSShPTjVN3YF3A4I7ycxEcAhy7MtcSbZviZEGSuRy02NS+CiLW/Q7oQw==", "60b257de-cf69-42a4-bead-f1fc76ca642f" });
        }
    }
}
