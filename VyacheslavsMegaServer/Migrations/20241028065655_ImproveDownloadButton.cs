using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaServer.Migrations
{
    /// <inheritdoc />
    public partial class ImproveDownloadButton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fb32f4c4-4818-4552-b612-1ef123381062", "AQAAAAIAAYagAAAAEGjjumFWurs7EYSLTqwtbUvaMdfmTKbngWaDVnK9bGfrFWPDdLebPsI+So6dV+yVmg==", "53ba61f4-c0d8-48bd-a661-b5743e7561a6" });

            migrationBuilder.UpdateData(
                table: "MainPageData",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DownloadButtonTitle", "DownloadButtonUrl", "DownloadLocalResource" },
                values: new object[] { "Скачать", "", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fb9362d5-9546-42fa-b80d-e58543f8610d", "AQAAAAIAAYagAAAAEM1e5jtkfFggvRxckdMm5OkNU8WdZrAnm7TA9GFMiiG2a59TfoXUdc0LUYARr5tvog==", "5b47b990-4aff-4a47-8e7a-80122463a0c7" });
        }
    }
}
