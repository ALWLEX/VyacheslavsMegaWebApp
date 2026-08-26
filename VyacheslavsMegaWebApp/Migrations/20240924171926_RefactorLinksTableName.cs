using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorLinksTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.CreateTable(
                name: "ContactLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactLinks_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51c3cb26-b3e1-443d-a26e-490b90453641", "AQAAAAIAAYagAAAAEGfRzEQzjm09XydhKkivnFnzZf1mDg9bMhtuz2yVkjl0u4cWFDQctEBiPiyYgcgq2w==", "e5be32bc-39c0-420a-9c43-4d703bbcf167" });

            migrationBuilder.InsertData(
                table: "ContactLinks",
                columns: new[] { "Id", "ContactId", "Content", "Description", "Url" },
                values: new object[,]
                {
                    { 1, 1, "Вячеслав Костырев", "VK: ", "https://vk.com/sansei_1" },
                    { 2, 1, "@vyacheslav_minecraft", "Telegram: ", "https://t.me/vyacheslav_minecraft" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactLinks_ContactId",
                table: "ContactLinks",
                column: "ContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactLinks");

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dc140be-3b1a-4077-9247-927be5ec7b5e", "AQAAAAIAAYagAAAAEHF1X+sTY6FbBycEx4Hnp2Spk2MsEv28ZbsoW+VQBznWVGnRub8gp0ljRghxPo4bSQ==", "773641d3-6950-4968-b39c-713c2265367b" });

            migrationBuilder.InsertData(
                table: "Links",
                columns: new[] { "Id", "ContactId", "Content", "Description", "Url" },
                values: new object[] { 1, 1, "Вячеслав Костырев", "VK: ", "https://vk.com/sansei_1" });

            migrationBuilder.CreateIndex(
                name: "IX_Links_ContactId",
                table: "Links",
                column: "ContactId");
        }
    }
}
