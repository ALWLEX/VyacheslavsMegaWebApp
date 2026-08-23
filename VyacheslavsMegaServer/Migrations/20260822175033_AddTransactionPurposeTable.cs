using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VyacheslavsMegaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionPurposeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionPurposeId",
                table: "FundTransactions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "TransactionPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPurposes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71407eed-1a56-467f-bcdb-d270ce07d612", "AQAAAAIAAYagAAAAENnw4GUfHNkukFqRPLw/ERP1QA4iWxnvmbDgxbXg0yu5W53NMpnuRN1j6CtsLfboZA==", "9c64845b-4313-49cc-a6cc-801ab81930b5" });

            migrationBuilder.InsertData(
                table: "TransactionPurposes",
                columns: new[] { "Id", "Description", "Name", "Type" },
                values: new object[,]
                {
                    { 1, null, "Пожертвование", 0 },
                    { 2, null, "Покупка комплектующих сервера", 1 }
                });

            migrationBuilder.Sql(
                """
                SET SQL_SAFE_UPDATES = 0;
                UPDATE FundTransactions SET 
                Note =
                    CASE
                        WHEN Note IS NULL OR Note = '' THEN Reason
                        ELSE CONCAT(Reason, ' (', Note, ')')
                    END,
                TransactionPurposeId = 
                    CASE
                        WHEN Amount > 0 THEN 1
                        ELSE 2
                    END;
                SET SQL_SAFE_UPDATES = 1;
                """
            );

            migrationBuilder.CreateIndex(
                name: "IX_FundTransactions_TransactionPurposeId",
                table: "FundTransactions",
                column: "TransactionPurposeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FundTransactions_TransactionPurposes_TransactionPurposeId",
                table: "FundTransactions",
                column: "TransactionPurposeId",
                principalTable: "TransactionPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "FundTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundTransactions_TransactionPurposes_TransactionPurposeId",
                table: "FundTransactions");

            migrationBuilder.DropTable(
                name: "TransactionPurposes");

            migrationBuilder.DropIndex(
                name: "IX_FundTransactions_TransactionPurposeId",
                table: "FundTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionPurposeId",
                table: "FundTransactions");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "FundTransactions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                """
                UPDATE FundTransactions SET
                Reason = REGEXP_SUBSTR(Note, '^[^\\(]*'),
                Note = REGEXP_SUBSTR(Note, '(?<=\\()[^)]+(?=\\))');
                """
            );


            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "270defe0-6b04-4433-bef0-b3e7654b22b7", "AQAAAAIAAYagAAAAEIjF3mA9fh8VQFRVNsoVeGNx+NGxEECZQRZcUoSJhO6di7B7gw6CEAFfdOux2q9Etg==", "6ab088d3-1fac-4cf5-bca1-7d360c0286df" });
        }
    }
}
