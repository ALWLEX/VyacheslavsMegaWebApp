using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddModRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "03d4ba1e-afef-4e77-849d-295bd86148db", null, "Moderator", "MODERATOR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "932c242a-61fa-4fa6-8cf6-d8474089f3ec", "AQAAAAIAAYagAAAAEDUHF5y++2bQziyGnqwzKXBVe68/jRqx28a+N5fTGaJ+8NP/XqyIxzQUe/TXb6oFBw==", "001a8bd8-f8c4-409b-8e08-92f4dacdc426" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03d4ba1e-afef-4e77-849d-295bd86148db");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fb32f4c4-4818-4552-b612-1ef123381062", "AQAAAAIAAYagAAAAEGjjumFWurs7EYSLTqwtbUvaMdfmTKbngWaDVnK9bGfrFWPDdLebPsI+So6dV+yVmg==", "53ba61f4-c0d8-48bd-a661-b5743e7561a6" });
        }
    }
}
