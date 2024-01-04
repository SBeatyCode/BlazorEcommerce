using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorEcommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Gideon the Ninth is a 2019 science fantasy novel by the New Zealand writer Tamsyn Muir. It is Muir's debut novel and the first in her Locked Tomb series", "https://upload.wikimedia.org/wikipedia/en/thumb/6/66/Gideon_the_Ninth_-_US_hardback_print_cover.jpg/220px-Gideon_the_Ninth_-_US_hardback_print_cover.jpg", "Gideon the Ninth", 12.99m },
                    { 2, "Harrow the Ninth is a 2020 science fantasy novel by the New Zealand writer Tamsyn Muir. It is the second in Muir's Locked Tomb series", "https://upload.wikimedia.org/wikipedia/en/thumb/0/02/Harrow_the_Ninth.jpg/220px-Harrow_the_Ninth.jpg", "Harrow the Ninth", 12.99m },
                    { 3, "Nona the Ninth is a 2022 science fantasy novel by the New Zealand writer Tamsyn Muir. It is the third book in the Locked Tomb series", "https://upload.wikimedia.org/wikipedia/en/thumb/8/81/Nona_the_Ninth.jpg/220px-Nona_the_Ninth.jpg", "Nona the Ninth", 15.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
