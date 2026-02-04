using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddServerVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServerVersion",
                table: "MainPageData",
                type: "longtext",
                nullable: false);

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
                column: "ServerVersion",
                value: "1.21.11");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "MainPageData");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "271bc70d-03c9-467a-acaa-cfb094664433", "AQAAAAIAAYagAAAAELDf/qOuQX2XwSq+i0yCz2/4H96EyU1x130n3FNMST2GxFwzvZNNTOdirVcAVXXkyg==", "022680bc-5a50-48a9-a45f-f520b6317944" });
        }
    }
}
