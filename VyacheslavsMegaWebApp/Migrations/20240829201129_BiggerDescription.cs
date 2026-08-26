using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class BiggerDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MainPageData",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b32f874-ceb3-4b45-a932-198503a565ea", "AQAAAAIAAYagAAAAEGOqtblJV5+9CRavpSaMCZ0VBSAY20IZQ1ll2J+OIwNrb6kRW67MqUAhcwcn4jFw4g==", "5a613399-cf4c-4525-8a4a-8c042e647331" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MainPageData",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a0f36c9-163e-42f8-ba5b-dc50d193de3d", "AQAAAAIAAYagAAAAEBnGEUhESC/bRbTZgH8RM9Sd3dsUhm90D7Ud09vQmUfzhNRXbyVN28gkDvGCya2dPQ==", "18dc2b44-a5f4-42e3-abe1-f81e114a4d0d" });
        }
    }
}
