using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ErrMessageInMainPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "MainPageData",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowErrorMessage",
                table: "MainPageData",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a0f36c9-163e-42f8-ba5b-dc50d193de3d", "AQAAAAIAAYagAAAAEBnGEUhESC/bRbTZgH8RM9Sd3dsUhm90D7Ud09vQmUfzhNRXbyVN28gkDvGCya2dPQ==", "18dc2b44-a5f4-42e3-abe1-f81e114a4d0d" });

            migrationBuilder.UpdateData(
                table: "MainPageData",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ErrorMessage", "ShowErrorMessage" },
                values: new object[] { "", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "MainPageData");

            migrationBuilder.DropColumn(
                name: "ShowErrorMessage",
                table: "MainPageData");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40b48334-134c-4e5e-a453-96c4a3972e15", "AQAAAAIAAYagAAAAEFF/htli9tizM8YH6ykcuGIuPZAC6A4Pn4akw18uwOi6bvxzlRZf7dDs3Yswk+VE1Q==", "8bb0f27f-38f9-466a-bbc8-a9778dd1e72a" });
        }
    }
}
