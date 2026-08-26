using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddServerMediaLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerMediaLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ShowLink = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LogoFileName = table.Column<string>(type: "longtext", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerMediaLinks", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fb9362d5-9546-42fa-b80d-e58543f8610d", "AQAAAAIAAYagAAAAEM1e5jtkfFggvRxckdMm5OkNU8WdZrAnm7TA9GFMiiG2a59TfoXUdc0LUYARr5tvog==", "5b47b990-4aff-4a47-8e7a-80122463a0c7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerMediaLinks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0edbdebc-d1f4-4dd6-8e58-8c102a0bc050", "AQAAAAIAAYagAAAAEMe+mWxc/AdSiFOR1L+OJj69oqOvxu5SJgG6EyggWi+ExqBuEw4GnZBAjnXAFAxp1A==", "b6231d23-1948-4d6d-9c97-c54e75dfec47" });
        }
    }
}
