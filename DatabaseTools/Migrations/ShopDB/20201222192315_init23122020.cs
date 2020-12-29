using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init23122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeoAlias",
                table: "TAGS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "TAGS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "TAGS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoAlias",
                table: "PRODUCTS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "PRODUCTS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "PRODUCTS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoAlias",
                table: "PRODUCT_TYPES",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "PRODUCT_TYPES",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "PRODUCT_TYPES",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoAlias",
                table: "MATERIALS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "MATERIALS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "MATERIALS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoAlias",
                table: "CUSTOMERS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CUSTOMERS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "CUSTOMERS",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoAlias",
                table: "CATEGORIES",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CATEGORIES",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "CATEGORIES",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeoAlias",
                table: "TAGS");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "TAGS");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "TAGS");

            migrationBuilder.DropColumn(
                name: "SeoAlias",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "SeoAlias",
                table: "PRODUCT_TYPES");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "PRODUCT_TYPES");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "PRODUCT_TYPES");

            migrationBuilder.DropColumn(
                name: "SeoAlias",
                table: "MATERIALS");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "MATERIALS");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "MATERIALS");

            migrationBuilder.DropColumn(
                name: "SeoAlias",
                table: "CUSTOMERS");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CUSTOMERS");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "CUSTOMERS");

            migrationBuilder.DropColumn(
                name: "SeoAlias",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "CATEGORIES");
        }
    }
}
