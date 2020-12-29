using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init18122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiemCustomerDanhGia",
                table: "CART_PRODUCT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TinhTrangChiTiet",
                table: "CART_PRODUCT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiemCustomerDanhGia",
                table: "CART_PRODUCT");

            migrationBuilder.DropColumn(
                name: "TinhTrangChiTiet",
                table: "CART_PRODUCT");
        }
    }
}
