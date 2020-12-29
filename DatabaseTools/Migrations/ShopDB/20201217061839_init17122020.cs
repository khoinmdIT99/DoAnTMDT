using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init17122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TinhTrang",
                table: "CUSTOMERS",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "CARTS",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TinhTrangDanhGiaCustomer",
                table: "CARTS",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrang",
                table: "CUSTOMERS");

            migrationBuilder.DropColumn(
                name: "TinhTrangDanhGiaCustomer",
                table: "CARTS");

            migrationBuilder.AlterColumn<int>(
                name: "STATUS",
                table: "CARTS",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
