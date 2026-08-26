using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddShowDownloadButton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowDownloadButton",
                table: "MainPageData",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3deafef9-3642-40d3-ba74-d503bf685727", "AQAAAAIAAYagAAAAEJ7uu0QmjMPSShPTjVN3YF3A4I7ycxEcAhy7MtcSbZviZEGSuRy02NS+CiLW/Q7oQw==", "60b257de-cf69-42a4-bead-f1fc76ca642f" });

            migrationBuilder.UpdateData(
                table: "MainPageData",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShowDownloadButton",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowDownloadButton",
                table: "MainPageData");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b32f874-ceb3-4b45-a932-198503a565ea", "AQAAAAIAAYagAAAAEGOqtblJV5+9CRavpSaMCZ0VBSAY20IZQ1ll2J+OIwNrb6kRW67MqUAhcwcn4jFw4g==", "5a613399-cf4c-4525-8a4a-8c042e647331" });
        }
    }
}
