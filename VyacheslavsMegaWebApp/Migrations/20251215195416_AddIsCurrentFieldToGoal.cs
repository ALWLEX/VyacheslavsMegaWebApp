using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCurrentFieldToGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Goals",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Goals",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "FundTransactions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b4d40c6d-dbcb-4a78-9992-cd59e11ed78b", "AQAAAAIAAYagAAAAEJZaBTMzCLAgGAFrc3rsSmzH4m+79fuX7YCCs/EoHwh7bsjObpGOtM/cc5qIOVAHPA==", "ec0415b2-922d-495a-a96e-1666fb398fa6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Goals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Goals",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "FundTransactions",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96d9e72d-56b0-4313-b0d6-5753616a2ffd", "AQAAAAIAAYagAAAAEJKB91on5Zts5FGHaXgynbTlYrjN72EEFGw2zTjQlVbyJLzYfYbt+d3cjA/c4Y9BUA==", "03f1268f-a02b-482e-baa5-4c52707defd5" });
        }
    }
}
