using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTransactionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "FundTransactions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96d9e72d-56b0-4313-b0d6-5753616a2ffd", "AQAAAAIAAYagAAAAEJKB91on5Zts5FGHaXgynbTlYrjN72EEFGw2zTjQlVbyJLzYfYbt+d3cjA/c4Y9BUA==", "03f1268f-a02b-482e-baa5-4c52707defd5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "FundTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbcf503b-20c3-4121-872a-6e9faf8c6b5f", "AQAAAAIAAYagAAAAECGGEUylN02GEy8QFN3dI0aajPYYDIfD7JOA4O05Z5r2YxQwOi2q/g8rEUDb1PCbEw==", "a9b446b9-1c2c-4e6f-ad7d-986bbc2f3186" });
        }
    }
}
