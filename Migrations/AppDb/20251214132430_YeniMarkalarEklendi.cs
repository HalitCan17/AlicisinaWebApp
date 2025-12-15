using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AlicisinaWebApp.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class YeniMarkalarEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "brandId", "CategoryId", "brandName" },
                values: new object[,]
                {
                    { 4, 2, "Suzuki" },
                    { 5, 2, "Honda" },
                    { 6, 2, "Mondial" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "brandId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "brandId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "brandId",
                keyValue: 6);
        }
    }
}
