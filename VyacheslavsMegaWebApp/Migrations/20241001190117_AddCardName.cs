using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCardName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "DonationCards",
                type: "longtext",
                nullable: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0edbdebc-d1f4-4dd6-8e58-8c102a0bc050", "AQAAAAIAAYagAAAAEMe+mWxc/AdSiFOR1L+OJj69oqOvxu5SJgG6EyggWi+ExqBuEw4GnZBAjnXAFAxp1A==", "b6231d23-1948-4d6d-9c97-c54e75dfec47" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardName",
                table: "DonationCards");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "21d2f81c-cdf0-430c-9c11-293f9d7d3a76", "AQAAAAIAAYagAAAAEA1KUDVzCn6c2xZ3lkXQmzd2yuGNcWtPJIt9Lglzmp/ZdIEFZ0niX21VxPyIoY++TA==", "c2a28b04-1b16-4923-8767-e6174676ad8a" });
        }
    }
}
