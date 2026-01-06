using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VyacheslavsMegaServer.Migrations
{
    /// <inheritdoc />
    public partial class NullableGoalImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LogoFileName",
                table: "Goals",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Goals",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "271bc70d-03c9-467a-acaa-cfb094664433", "AQAAAAIAAYagAAAAELDf/qOuQX2XwSq+i0yCz2/4H96EyU1x130n3FNMST2GxFwzvZNNTOdirVcAVXXkyg==", "022680bc-5a50-48a9-a45f-f520b6317944" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LogoFileName",
                table: "Goals",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Goals",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4bc75a9-ca48-45aa-a3f2-2ea5886ce303",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b4d40c6d-dbcb-4a78-9992-cd59e11ed78b", "AQAAAAIAAYagAAAAEJZaBTMzCLAgGAFrc3rsSmzH4m+79fuX7YCCs/EoHwh7bsjObpGOtM/cc5qIOVAHPA==", "ec0415b2-922d-495a-a96e-1666fb398fa6" });
        }
    }
}
