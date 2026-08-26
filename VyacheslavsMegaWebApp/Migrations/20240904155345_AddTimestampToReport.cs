using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampToReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimestampCreated",
                table: "UserReports",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cee506bf-58dd-48c7-8e3f-fe90f1a87b4f", "AQAAAAIAAYagAAAAEOXql5qz393LYy/E7zOP4a+PzKhYG9d3EuuEoECV9sQ/xFB/K2k3XWjfbhqnrxvwLg==", "01db8b5e-bd4e-411d-a2fa-a0c6db39b1cf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimestampCreated",
                table: "UserReports");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f21f2ff4-f4a7-40c3-92d8-a5b186b20f1f", "AQAAAAIAAYagAAAAEIR4aROJ3sRDyBKf/HFPMG8xk7z76cPbQtgZDo4gAePbXzMF9VJMNYyVpO91cvP2KA==", "361172ba-1677-44ef-9454-27b989ed0285" });
        }
    }
}
