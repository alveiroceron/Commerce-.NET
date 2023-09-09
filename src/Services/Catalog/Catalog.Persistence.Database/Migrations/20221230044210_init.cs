using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Catalog");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Catalog",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                schema: "Catalog",
                columns: table => new
                {
                    ProducInStockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.ProducInStockId);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Catalog",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "Products",
                columns: new[] { "ProductId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Description for ptoduct 1", "Product 1", 907m },
                    { 2, "Description for ptoduct 2", "Product 2", 905m },
                    { 3, "Description for ptoduct 3", "Product 3", 276m },
                    { 4, "Description for ptoduct 4", "Product 4", 387m },
                    { 5, "Description for ptoduct 5", "Product 5", 351m },
                    { 6, "Description for ptoduct 6", "Product 6", 511m },
                    { 7, "Description for ptoduct 7", "Product 7", 162m },
                    { 8, "Description for ptoduct 8", "Product 8", 758m },
                    { 9, "Description for ptoduct 9", "Product 9", 136m },
                    { 10, "Description for ptoduct 10", "Product 10", 281m },
                    { 11, "Description for ptoduct 11", "Product 11", 722m },
                    { 12, "Description for ptoduct 12", "Product 12", 490m },
                    { 13, "Description for ptoduct 13", "Product 13", 884m },
                    { 14, "Description for ptoduct 14", "Product 14", 782m },
                    { 15, "Description for ptoduct 15", "Product 15", 293m },
                    { 16, "Description for ptoduct 16", "Product 16", 735m },
                    { 17, "Description for ptoduct 17", "Product 17", 205m },
                    { 18, "Description for ptoduct 18", "Product 18", 845m },
                    { 19, "Description for ptoduct 19", "Product 19", 431m },
                    { 20, "Description for ptoduct 20", "Product 20", 947m },
                    { 21, "Description for ptoduct 21", "Product 21", 230m },
                    { 22, "Description for ptoduct 22", "Product 22", 731m },
                    { 23, "Description for ptoduct 23", "Product 23", 548m },
                    { 24, "Description for ptoduct 24", "Product 24", 439m },
                    { 25, "Description for ptoduct 25", "Product 25", 367m },
                    { 26, "Description for ptoduct 26", "Product 26", 365m },
                    { 27, "Description for ptoduct 27", "Product 27", 264m },
                    { 28, "Description for ptoduct 28", "Product 28", 981m },
                    { 29, "Description for ptoduct 29", "Product 29", 644m },
                    { 30, "Description for ptoduct 30", "Product 30", 887m },
                    { 31, "Description for ptoduct 31", "Product 31", 687m },
                    { 32, "Description for ptoduct 32", "Product 32", 208m },
                    { 33, "Description for ptoduct 33", "Product 33", 191m },
                    { 34, "Description for ptoduct 34", "Product 34", 188m },
                    { 35, "Description for ptoduct 35", "Product 35", 282m },
                    { 36, "Description for ptoduct 36", "Product 36", 622m },
                    { 37, "Description for ptoduct 37", "Product 37", 863m },
                    { 38, "Description for ptoduct 38", "Product 38", 600m },
                    { 39, "Description for ptoduct 39", "Product 39", 638m },
                    { 40, "Description for ptoduct 40", "Product 40", 566m },
                    { 41, "Description for ptoduct 41", "Product 41", 581m },
                    { 42, "Description for ptoduct 42", "Product 42", 646m },
                    { 43, "Description for ptoduct 43", "Product 43", 279m },
                    { 44, "Description for ptoduct 44", "Product 44", 958m },
                    { 45, "Description for ptoduct 45", "Product 45", 393m },
                    { 46, "Description for ptoduct 46", "Product 46", 433m },
                    { 47, "Description for ptoduct 47", "Product 47", 795m },
                    { 48, "Description for ptoduct 48", "Product 48", 128m },
                    { 49, "Description for ptoduct 49", "Product 49", 848m },
                    { 50, "Description for ptoduct 50", "Product 50", 683m },
                    { 51, "Description for ptoduct 51", "Product 51", 218m },
                    { 52, "Description for ptoduct 52", "Product 52", 874m },
                    { 53, "Description for ptoduct 53", "Product 53", 415m },
                    { 54, "Description for ptoduct 54", "Product 54", 330m },
                    { 55, "Description for ptoduct 55", "Product 55", 570m },
                    { 56, "Description for ptoduct 56", "Product 56", 213m },
                    { 57, "Description for ptoduct 57", "Product 57", 664m },
                    { 58, "Description for ptoduct 58", "Product 58", 951m },
                    { 59, "Description for ptoduct 59", "Product 59", 222m },
                    { 60, "Description for ptoduct 60", "Product 60", 248m },
                    { 61, "Description for ptoduct 61", "Product 61", 548m },
                    { 62, "Description for ptoduct 62", "Product 62", 633m },
                    { 63, "Description for ptoduct 63", "Product 63", 424m },
                    { 64, "Description for ptoduct 64", "Product 64", 605m },
                    { 65, "Description for ptoduct 65", "Product 65", 424m },
                    { 66, "Description for ptoduct 66", "Product 66", 964m },
                    { 67, "Description for ptoduct 67", "Product 67", 378m },
                    { 68, "Description for ptoduct 68", "Product 68", 703m },
                    { 69, "Description for ptoduct 69", "Product 69", 705m },
                    { 70, "Description for ptoduct 70", "Product 70", 507m },
                    { 71, "Description for ptoduct 71", "Product 71", 628m },
                    { 72, "Description for ptoduct 72", "Product 72", 902m },
                    { 73, "Description for ptoduct 73", "Product 73", 773m },
                    { 74, "Description for ptoduct 74", "Product 74", 982m },
                    { 75, "Description for ptoduct 75", "Product 75", 870m },
                    { 76, "Description for ptoduct 76", "Product 76", 871m },
                    { 77, "Description for ptoduct 77", "Product 77", 391m },
                    { 78, "Description for ptoduct 78", "Product 78", 543m },
                    { 79, "Description for ptoduct 79", "Product 79", 905m },
                    { 80, "Description for ptoduct 80", "Product 80", 224m },
                    { 81, "Description for ptoduct 81", "Product 81", 650m },
                    { 82, "Description for ptoduct 82", "Product 82", 124m },
                    { 83, "Description for ptoduct 83", "Product 83", 863m },
                    { 84, "Description for ptoduct 84", "Product 84", 106m },
                    { 85, "Description for ptoduct 85", "Product 85", 658m },
                    { 86, "Description for ptoduct 86", "Product 86", 628m },
                    { 87, "Description for ptoduct 87", "Product 87", 865m },
                    { 88, "Description for ptoduct 88", "Product 88", 635m },
                    { 89, "Description for ptoduct 89", "Product 89", 321m },
                    { 90, "Description for ptoduct 90", "Product 90", 874m },
                    { 91, "Description for ptoduct 91", "Product 91", 169m },
                    { 92, "Description for ptoduct 92", "Product 92", 703m },
                    { 93, "Description for ptoduct 93", "Product 93", 526m },
                    { 94, "Description for ptoduct 94", "Product 94", 787m },
                    { 95, "Description for ptoduct 95", "Product 95", 531m },
                    { 96, "Description for ptoduct 96", "Product 96", 446m },
                    { 97, "Description for ptoduct 97", "Product 97", 283m },
                    { 98, "Description for ptoduct 98", "Product 98", 460m },
                    { 99, "Description for ptoduct 99", "Product 99", 624m }
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "Stocks",
                columns: new[] { "ProducInStockId", "ProductId", "Stock" },
                values: new object[,]
                {
                    { 1, 1, 12 },
                    { 2, 2, 12 },
                    { 3, 3, 10 },
                    { 4, 4, 13 },
                    { 5, 5, 10 },
                    { 6, 6, 7 },
                    { 7, 7, 18 },
                    { 8, 8, 9 },
                    { 9, 9, 11 },
                    { 10, 10, 14 },
                    { 11, 11, 17 },
                    { 12, 12, 10 },
                    { 13, 13, 11 },
                    { 14, 14, 6 },
                    { 15, 15, 14 },
                    { 16, 16, 19 },
                    { 17, 17, 10 },
                    { 18, 18, 16 },
                    { 19, 19, 6 },
                    { 20, 20, 14 },
                    { 21, 21, 18 },
                    { 22, 22, 10 },
                    { 23, 23, 10 },
                    { 24, 24, 16 },
                    { 25, 25, 8 },
                    { 26, 26, 7 },
                    { 27, 27, 14 },
                    { 28, 28, 11 },
                    { 29, 29, 11 },
                    { 30, 30, 14 },
                    { 31, 31, 19 },
                    { 32, 32, 7 },
                    { 33, 33, 16 },
                    { 34, 34, 3 },
                    { 35, 35, 11 },
                    { 36, 36, 19 },
                    { 37, 37, 12 },
                    { 38, 38, 17 },
                    { 39, 39, 7 },
                    { 40, 40, 8 },
                    { 41, 41, 19 },
                    { 42, 42, 13 },
                    { 43, 43, 16 },
                    { 44, 44, 17 },
                    { 45, 45, 16 },
                    { 46, 46, 0 },
                    { 47, 47, 1 },
                    { 48, 48, 14 },
                    { 49, 49, 1 },
                    { 50, 50, 5 },
                    { 51, 51, 4 },
                    { 52, 52, 5 },
                    { 53, 53, 7 },
                    { 54, 54, 9 },
                    { 55, 55, 17 },
                    { 56, 56, 17 },
                    { 57, 57, 8 },
                    { 58, 58, 15 },
                    { 59, 59, 1 },
                    { 60, 60, 8 },
                    { 61, 61, 16 },
                    { 62, 62, 8 },
                    { 63, 63, 2 },
                    { 64, 64, 4 },
                    { 65, 65, 1 },
                    { 66, 66, 0 },
                    { 67, 67, 2 },
                    { 68, 68, 19 },
                    { 69, 69, 15 },
                    { 70, 70, 5 },
                    { 71, 71, 16 },
                    { 72, 72, 12 },
                    { 73, 73, 14 },
                    { 74, 74, 14 },
                    { 75, 75, 15 },
                    { 76, 76, 6 },
                    { 77, 77, 12 },
                    { 78, 78, 18 },
                    { 79, 79, 7 },
                    { 80, 80, 17 },
                    { 81, 81, 8 },
                    { 82, 82, 15 },
                    { 83, 83, 17 },
                    { 84, 84, 18 },
                    { 85, 85, 18 },
                    { 86, 86, 10 },
                    { 87, 87, 2 },
                    { 88, 88, 2 },
                    { 89, 89, 19 },
                    { 90, 90, 17 },
                    { 91, 91, 10 },
                    { 92, 92, 9 },
                    { 93, 93, 9 },
                    { 94, 94, 11 },
                    { 95, 95, 0 },
                    { 96, 96, 6 },
                    { 97, 97, 5 },
                    { 98, 98, 7 },
                    { 99, 99, 16 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                schema: "Catalog",
                table: "Stocks",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocks",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Catalog");
        }
    }
}
