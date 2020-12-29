using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init12252020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TinhTrangDanhGiaCustomer",
                table: "CARTS",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "CARTS",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATE_AT",
                table: "CARTS",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TinhTrangChiTiet",
                table: "CART_PRODUCT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayGiao",
                table: "CART_PRODUCT",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdTaiKhoanDanhGia = table.Column<string>(nullable: true),
                    Diem = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGias_CUSTOMERS_IdTaiKhoanDanhGia",
                        column: x => x.IdTaiKhoanDanhGia,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__DonHang__129584AC539A1D8D",
                table: "CARTS",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_IdTaiKhoanDanhGia",
                table: "DanhGias",
                column: "IdTaiKhoanDanhGia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropIndex(
                name: "UQ__DonHang__129584AC539A1D8D",
                table: "CARTS");

            migrationBuilder.AlterColumn<string>(
                name: "TinhTrangDanhGiaCustomer",
                table: "CARTS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "STATUS",
                table: "CARTS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATE_AT",
                table: "CARTS",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TinhTrangChiTiet",
                table: "CART_PRODUCT",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayGiao",
                table: "CART_PRODUCT",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }
    }
}
