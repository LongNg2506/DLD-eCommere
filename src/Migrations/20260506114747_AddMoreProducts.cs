using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Discount", "GalleryImages", "ImageUrl", "IsActive", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 17, 1, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tag Heuer Monaco, iconic square case, automatic chronograph", 0m, "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80", true, "Tag Heuer Monaco", 52000000m, 4, null },
                    { 18, 1, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bulova Precisionist, ultra-precise quartz movement, sapphire crystal", 10m, "https://images.unsplash.com/photo-1622434641406-a158123450f9?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80", "https://images.unsplash.com/photo-1622434641406-a158123450f9?w=600&q=80", true, "Bulova Precisionist", 7800000m, 16, null },
                    { 19, 1, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frederique Constant Classics, elegant dress watch, automatic movement", 5m, "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80", true, "Frederique Constant Classics", 18500000m, 6, null },
                    { 20, 2, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Baume et Mercier Classima, ladies elegant dress watch, quartz", 0m, "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80", "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80", true, "Baume et Mercier Classima", 12800000m, 9, null },
                    { 21, 2, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tissot Chemises, classic ladies watch, sapphire crystal", 15m, "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80", "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80", true, "Tissot Chemises", 5600000m, 21, null },
                    { 22, 2, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Longines DolceVita, ladies luxury watch, rectangular case, automatic", 0m, "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80", true, "Longines DolceVita", 17000000m, 7, null },
                    { 23, 1, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cartier Santos de Cartier, legendary aviator watch, steel bracelet", 0m, "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80", "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80", true, "Cartier Santos de Cartier", 62000000m, 2, null },
                    { 24, 1, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seiko Prospex Turtle, legendary dive watch, automatic, 200m water resistant", 0m, "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80", "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80", true, "Seiko Prospex Turtle", 8200000m, 25, null },
                    { 25, 2, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omega De Ville Prestige, ladies luxury watch, Co-Axial movement", 0m, "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80", true, "Omega De Ville Prestige", 38000000m, 5, null },
                    { 26, 1, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Certina DS Action, sporty dive watch, ceramic bezel, automatic", 8m, "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80", "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80", true, "Certina DS Action", 9500000m, 18, null },
                    { 27, 1, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hamilton Jazzmaster, American spirit, automatic, exhibition caseback", 0m, "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80", "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80", true, "Hamilton Jazzmaster", 14000000m, 8, null },
                    { 28, 1, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mido Baroncelli, elegant dress watch, automatic, slim profile", 12m, "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80", "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", true, "Mido Baroncelli", 11200000m, 11, null },
                    { 29, 1, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rado Captain Cook, iconic dive watch, ceramic case, automatic", 0m, "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80", true, "Rado Captain Cook", 24500000m, 6, null },
                    { 30, 1, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oris Big Crown Pointer Date, pilot watch, bronze case, automatic", 0m, "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80", "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80", true, "Oris Big Crown", 29000000m, 3, null },
                    { 31, 2, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gucci G-Timeless, ladies luxury watch, interlocking G motif", 20m, "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80", "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80", true, "Gucci G-Timeless", 18000000m, 14, null },
                    { 32, 2, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chopard Happy Sport, ladies luxury watch, moving diamonds, steel bracelet", 0m, "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80", "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80", true, "Chopard Happy Sport", 42000000m, 4, null },
                    { 33, 3, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tissot Gentleman couple set, matching elegant design, automatic", 10m, "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80", "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80", true, "Tissot Couple Gentleman", 9200000m, 15, null },
                    { 34, 3, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seiko Presage couple set, Japanese craftsmanship, automatic, enamel dial", 5m, "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80", "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80", true, "Seiko Presage Couple", 13400000m, 12, null },
                    { 35, 1, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Citizen Eco-Drive Promaster, dive watch, light-powered, 200m water resistant", 0m, "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80", "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80", true, "Citizen Eco-Drive Promaster", 6800000m, 19, null },
                    { 36, 1, new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Longines Conquest Classic, sporty elegance, automatic, 30 bar water resistant", 0m, "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80", "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80", true, "Longines Conquest Classic", 19500000m, 8, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36);
        }
    }
}
