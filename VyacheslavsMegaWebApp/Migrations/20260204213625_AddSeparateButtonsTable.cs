using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSeparateButtonsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadButtonTitle",
                table: "MainPageData");

            migrationBuilder.DropColumn(
                name: "DownloadButtonUrl",
                table: "MainPageData");

            migrationBuilder.DropColumn(
                name: "DownloadLocalResource",
                table: "MainPageData");

            migrationBuilder.DropColumn(
                name: "ShowDownloadButton",
                table: "MainPageData");

            migrationBuilder.CreateTable(
                name: "DownloadButtons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false),
                    IsResourceLocal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShowButton = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadButtons", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "270defe0-6b04-4433-bef0-b3e7654b22b7", "AQAAAAIAAYagAAAAEIjF3mA9fh8VQFRVNsoVeGNx+NGxEECZQRZcUoSJhO6di7B7gw6CEAFfdOux2q9Etg==", "6ab088d3-1fac-4cf5-bca1-7d360c0286df" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadButtons");

            migrationBuilder.AddColumn<string>(
                name: "DownloadButtonTitle",
                table: "MainPageData",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "DownloadButtonUrl",
                table: "MainPageData",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "DownloadLocalResource",
                table: "MainPageData",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

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
                values: new object[] { "3ad85db5-54be-412d-aadb-b4bb06ec5050", "AQAAAAIAAYagAAAAEP3zMy9hmYa5rmQw9iQHo3bPw94zKnY57DFhcGeV0oAx/SJ/0rubWe6y6RiwxgPD/A==", "b07279fc-2968-4bfe-b04d-36278310bebc" });

            migrationBuilder.UpdateData(
                table: "MainPageData",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DownloadButtonTitle", "DownloadButtonUrl", "DownloadLocalResource", "ShowDownloadButton" },
                values: new object[] { "Скачать", "", false, false });
        }
    }
}
