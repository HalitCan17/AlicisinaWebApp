using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AlicisinaWebApp.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitialApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    categoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    brandId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    brandName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.brandId);
                    table.ForeignKey(
                        name: "FK_Brands_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    vehicleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    announcementDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    color = table.Column<string>(type: "TEXT", nullable: false),
                    year = table.Column<int>(type: "INTEGER", nullable: false),
                    serie = table.Column<string>(type: "TEXT", nullable: false),
                    fuelType = table.Column<string>(type: "TEXT", nullable: false),
                    gear = table.Column<string>(type: "TEXT", nullable: false),
                    kilometer = table.Column<decimal>(type: "TEXT", nullable: false),
                    bodyType = table.Column<string>(type: "TEXT", nullable: false),
                    enginePower = table.Column<string>(type: "TEXT", nullable: false),
                    engineDisplacement = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<decimal>(type: "TEXT", nullable: false),
                    imageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    vehicleStatus = table.Column<string>(type: "TEXT", nullable: false),
                    brandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true),
                    categoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.vehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Brands_brandId",
                        column: x => x.brandId,
                        principalTable: "Brands",
                        principalColumn: "brandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "categoryId", "categoryName" },
                values: new object[,]
                {
                    { 1, "Otomobil" },
                    { 2, "Motorsiklet" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "brandId", "CategoryId", "brandName" },
                values: new object[,]
                {
                    { 1, 1, "BMW" },
                    { 2, 1, "Mercedes" },
                    { 3, 2, "Yamaha" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "vehicleId", "AppUserId", "Description", "Title", "announcementDate", "bodyType", "brandId", "categoryId", "color", "engineDisplacement", "enginePower", "fuelType", "gear", "imageUrl", "kilometer", "model", "price", "serie", "vehicleStatus", "year" },
                values: new object[,]
                {
                    { 1, "3b2e3887-72a6-440a-b0f1-c50fa6322d9f", "BLA BLA BLA", "Sahibinden Temiz 3.20i SportLine", new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sedan", 1, 1, "Mavi", "1597cc", "170hp", "Benzinli", "Otomatik", "/images/320i.jpg", 56000m, "3.20i Sport Line", 2850000m, "3 Serisi", "İkinci el", 2022 },
                    { 2, "3b2e3887-72a6-440a-b0f1-c50fa6322d9f", "BLA BLA BLA", "Boyasız Tramersiz E180 Exclusive", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sedan", 2, 1, "Black", "1595cc", "156hp", "Benzinli", "Otomatik", "/images/e180.jpg", 100000m, "E180 Exclusive", 3250000m, "E Serisi", "İkinci el", 2017 },
                    { 3, "3b2e3887-72a6-440a-b0f1-c50fa6322d9f", "BLA BLA BLA", "Dosta Gider R25", new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Super Sport", 3, 2, "Red", "250cc", "25hp", "Benzinli", "Manuel", "/images/r25.jpeg", 15000m, "YZF R25 ABS", 210000m, "-", "İkinci el", 2021 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CategoryId",
                table: "Brands",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_brandId",
                table: "Vehicles",
                column: "brandId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_categoryId",
                table: "Vehicles",
                column: "categoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
