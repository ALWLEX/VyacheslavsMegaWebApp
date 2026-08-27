using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class NewIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "EmailConfirmCodeHash",
                table: "AspNetUsers",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmedCodeCreationTime",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmCodeHash", "EmailConfirmedCodeCreationTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a5df9ab-ea23-47a5-a702-59928042621c", null, null, "AQAAAAIAAYagAAAAEDDHhAPFdwjUltGX7G+ni4hfGrHg94Ix40YsEf7wg3CuEoMyZd7P037qzk9lf40f0Q==", "f521e49e-682a-4801-a6d6-6dcb47a44b80" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmCodeHash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmedCodeCreationTime",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71407eed-1a56-467f-bcdb-d270ce07d612", "AQAAAAIAAYagAAAAENnw4GUfHNkukFqRPLw/ERP1QA4iWxnvmbDgxbXg0yu5W53NMpnuRN1j6CtsLfboZA==", "9c64845b-4313-49cc-a6cc-801ab81930b5" });
        }
    }
}
