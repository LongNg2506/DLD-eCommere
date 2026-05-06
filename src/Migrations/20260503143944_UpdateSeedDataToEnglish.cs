using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Luxury and premium watches for men", "Men's Watches" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Elegant and stylish watches for women", "Women's Watches" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Matching watch pairs for couples", "Couple Watches" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Citizen Eco-Drive men's watch, light-powered battery", "Citizen Eco-Drive" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Seiko 5 men's watch, automatic movement", "Seiko 5" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Casio Sheen women's watch, sapphire crystal", "Casio Sheen" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Olym Pianus couple watch set, premium leather strap", "Olym Pianus Couple Set" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Tissot PRX men's watch, automatic, elegant design", "Tissot PRX" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Orient Bambino men's watch, sapphire crystal, leather strap", "Orient Bambino" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Daniel Wellington women's watch, minimalist design, NATO strap", "Daniel Wellington" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Longines Master Collection, automatic chronograph", "Longines Master Collection" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Fossil Jacqueline women's watch, elegant round dial", "Fossil Jacqueline" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Seiko couple watch set, perfectly matching design", "Seiko Couple Set" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Hamilton Khaki Field watch, automatic, 100m water resistant", "Hamilton Khaki Field" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Skagen Signatur women's watch, Danish minimalism", "Skagen Signatur" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Omega Seamaster, iconic dive watch, sapphire crystal", "Omega Seamaster" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Citizen Eco-Drive couple watch set, light-powered", "Citizen Couple Set" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Casio Edifice men's watch, Tough Solar, sapphire crystal", "Casio Edifice" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Cluse Minuit women's watch, French elegance, interchangeable strap", "Cluse Minuit" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Các loại đồng hồ dành cho nam giới", "Đồng hồ nam" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Các loại đồng hồ dành cho nữ giới", "Đồng hồ nữ" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ đôi dành cho nam và nữ", "Đồng hồ couple" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Citizen Eco-Drive nam, pin năng lượng ánh sáng", "Đồng hồ Citizen Eco-Drive" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Seiko 5 nam, automatic movement", "Đồng hồ Seiko 5" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Casio Sheen nữ, mặt kính sapphire", "Đồng hồ Casio Sheen" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Bộ đồng hồ đôi Olym Pianus, dây da cao cấp", "Đồng hồ Olym Pianus Couple" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Tissot PRX nam, automatic, thiết kế sang trọng", "Đồng hồ Tissot PRX" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Orient Bambino nam, mặt kính sapphire, dây da", "Đồng hồ Orient Bambino" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Daniel Wellington nữ, minimalist design, dây NATO", "Đồng hồ Daniel Wellington" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Longines Master Collection, automatic chronograph", "Đồng hồ Longines Master" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Fossil Jacqueline nữ, mặt tròn thanh lịch", "Đồng hồ Fossil Jacqueline" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Bộ đồng hồ đôi Seiko, thiết kế matching hoàn hảo", "Đồng hồ Couple Seiko" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Hamilton Khaki Field, automatic, chống nước 100m", "Đồng hồ Hamilton Khaki" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Skagen Signatur nữ, Danish minimalism", "Đồng hồ Skagen Signatur" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Omega Seamaster, iconic dive watch, sapphire crystal", "Đồng hồ Omega Seamaster" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Bộ đồng hồ đôi Citizen Eco-Drive, năng lượng ánh sáng", "Đồng hồ Couple Citizen" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Casio Edifice nam, Tough Solar, sapphire crystal", "Đồng hồ Casio Edifice" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Đồng hồ Cluse Minuit nữ, French elegance, interchangeable strap", "Đồng hồ Cluse Minuit" });
        }
    }
}
