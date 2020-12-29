using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init191220201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Actived",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExtraDiscount",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecial",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceAfter",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "PRODUCTS",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actived",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "ExtraDiscount",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "IsSpecial",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "PriceAfter",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "PRODUCTS");
        }
    }
}
