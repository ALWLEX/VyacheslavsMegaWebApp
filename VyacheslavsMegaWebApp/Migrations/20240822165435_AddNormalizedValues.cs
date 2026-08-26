using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db158bac-48d4-47c3-a6b0-c6d8b5c9ee55",
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40b48334-134c-4e5e-a453-96c4a3972e15", "VYA4SLAV4617@GMAIL.COM", "EVRAY", "AQAAAAIAAYagAAAAEFF/htli9tizM8YH6ykcuGIuPZAC6A4Pn4akw18uwOi6bvxzlRZf7dDs3Yswk+VE1Q==", "8bb0f27f-38f9-466a-bbc8-a9778dd1e72a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db158bac-48d4-47c3-a6b0-c6d8b5c9ee55",
                column: "NormalizedName",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b042f127-7911-4e95-91a3-01b0ae5356cb", null, null, "AQAAAAIAAYagAAAAEGCJiBdR1yHuoEmCa/e37JYBo1VKekIFqdw1lNTse6gsIPUg9mxqJiSUQhuF8UAZzA==", "7ffc24df-6f04-4611-a0f2-aee47a29ebdc" });
        }
    }
}
