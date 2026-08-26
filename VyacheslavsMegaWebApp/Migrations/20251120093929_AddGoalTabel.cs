using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LogoFileName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbcf503b-20c3-4121-872a-6e9faf8c6b5f", "AQAAAAIAAYagAAAAECGGEUylN02GEy8QFN3dI0aajPYYDIfD7JOA4O05Z5r2YxQwOi2q/g8rEUDb1PCbEw==", "a9b446b9-1c2c-4e6f-ad7d-986bbc2f3186" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e364ff1f-83b7-45ab-9248-40d075d1d24b", "AQAAAAIAAYagAAAAEAo5GMv5/DM7nvmOCNQcWu47nNlxyLYlFaxBAFQfHRzF7KU1oxSeAAEkJT+uLerEjA==", "ba54d010-9bb1-47d7-8754-64539d01bdb1" });
        }
    }
}
