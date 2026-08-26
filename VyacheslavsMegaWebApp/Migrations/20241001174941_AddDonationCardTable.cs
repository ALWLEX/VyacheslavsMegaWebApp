using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonationCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BankName = table.Column<string>(type: "longtext", nullable: false),
                    CardNumber = table.Column<string>(type: "longtext", nullable: false),
                    CardHolder = table.Column<string>(type: "longtext", nullable: false),
                    ColorUpRight = table.Column<string>(type: "longtext", nullable: false),
                    ColorDownLeft = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationCards", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "21d2f81c-cdf0-430c-9c11-293f9d7d3a76", "AQAAAAIAAYagAAAAEA1KUDVzCn6c2xZ3lkXQmzd2yuGNcWtPJIt9Lglzmp/ZdIEFZ0niX21VxPyIoY++TA==", "c2a28b04-1b16-4923-8767-e6174676ad8a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationCards");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1febdfc1-93e0-42ed-8031-5518e6d3f506", "AQAAAAIAAYagAAAAENwrJwMFDuTMsbnkS8HqhcVli9eCcawdo2XkP4RMyopxjxudweovYzcOeENKpXJFgA==", "3382a891-7a6b-40d8-90be-9034a917ca68" });
        }
    }
}
