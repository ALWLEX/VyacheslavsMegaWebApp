using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmCodeHash",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7e29b993-73c5-4a32-8cdd-e8dc0c7892ca", "AQAAAAIAAYagAAAAEPRGyNKDSt55tJNbfSuvdpIaHPcUzOARUsPC/Ra9tRDxPf2hdHkz+COm81Jp00iUcA==", "d48502c4-c8e3-4535-be3e-673c4dd5d5a7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "EmailConfirmCodeHash",
                table: "AspNetUsers",
                type: "longblob",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmCodeHash", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a5df9ab-ea23-47a5-a702-59928042621c", null, "AQAAAAIAAYagAAAAEDDHhAPFdwjUltGX7G+ni4hfGrHg94Ix40YsEf7wg3CuEoMyZd7P037qzk9lf40f0Q==", "f521e49e-682a-4801-a6d6-6dcb47a44b80" });
        }
    }
}
