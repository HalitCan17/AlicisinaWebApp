using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlicisinaWebApp.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class YeniMarkalarEklendi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "brandId", "CategoryId", "brandName" },
                values: new object[] { 7, 2, "Kawasaki" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "brandId",
                keyValue: 7);
        }
    }
}
