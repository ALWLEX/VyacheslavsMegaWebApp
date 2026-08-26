using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnersData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Subtitle = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    ShowPartner = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LogoFileName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PartnerLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerLinks_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1febdfc1-93e0-42ed-8031-5518e6d3f506", "AQAAAAIAAYagAAAAENwrJwMFDuTMsbnkS8HqhcVli9eCcawdo2XkP4RMyopxjxudweovYzcOeENKpXJFgA==", "3382a891-7a6b-40d8-90be-9034a917ca68" });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerLinks_PartnerId",
                table: "PartnerLinks",
                column: "PartnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerLinks");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51c3cb26-b3e1-443d-a26e-490b90453641", "AQAAAAIAAYagAAAAEGfRzEQzjm09XydhKkivnFnzZf1mDg9bMhtuz2yVkjl0u4cWFDQctEBiPiyYgcgq2w==", "e5be32bc-39c0-420a-9c43-4d703bbcf167" });
        }
    }
}
