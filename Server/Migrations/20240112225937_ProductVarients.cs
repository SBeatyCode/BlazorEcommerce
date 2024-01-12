using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorEcommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductVarients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductVarients",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVarients", x => new { x.ProductId, x.ProductTypeId });
                    table.ForeignKey(
                        name: "FK_ProductVarients_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVarients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Default" },
                    { 2, "Paperback" },
                    { 3, "Audiobook" },
                    { 4, "E-Book" },
                    { 5, "DVD" },
                    { 6, "Blu-Ray" },
                    { 7, "Digital" },
                    { 8, "Nintendo Switch" },
                    { 9, "PlayStation 4" },
                    { 10, "XBox One" },
                    { 11, "Game Console" }
                });

            migrationBuilder.InsertData(
                table: "ProductVarients",
                columns: new[] { "ProductId", "ProductTypeId", "OriginalPrice", "Price" },
                values: new object[,]
                {
                    { 1, 2, 18.99m, 12.99m },
                    { 1, 3, 28.99m, 18.99m },
                    { 1, 4, 15.99m, 9.99m },
                    { 2, 2, 18.99m, 12.99m },
                    { 2, 3, 28.99m, 18.99m },
                    { 2, 4, 15.99m, 9.99m },
                    { 3, 2, 18.99m, 12.99m },
                    { 3, 3, 28.99m, 18.99m },
                    { 3, 4, 15.99m, 9.99m },
                    { 4, 5, 9.99m, 5.99m },
                    { 4, 6, 20.50m, 10.99m },
                    { 5, 5, 15.99m, 9.99m },
                    { 5, 7, 18.99m, 12.99m },
                    { 6, 5, 18.99m, 8.99m },
                    { 6, 6, 19.99m, 19.99m },
                    { 6, 7, 25.99m, 9.99m },
                    { 7, 7, 59.99m, 9.99m },
                    { 7, 9, 59.99m, 20.99m },
                    { 7, 10, 59.99m, 22.99m },
                    { 8, 7, 20.99m, 20.99m },
                    { 8, 8, 20.99m, 16.99m },
                    { 8, 9, 20.99m, 16.99m },
                    { 9, 7, 15.99m, 59.99m },
                    { 9, 8, 28.99m, 59.99m },
                    { 10, 11, 79.99m, 79.99m },
                    { 11, 11, 15.99m, 99.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVarients_ProductTypeId",
                table: "ProductVarients",
                column: "ProductTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVarients");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 12.99m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 12.99m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 15.99m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "Price",
                value: 0m);
        }
    }
}
