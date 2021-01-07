using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init0501 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiemTichLuy_CARTS_HoadonId",
                table: "DiemTichLuy");

            migrationBuilder.DropForeignKey(
                name: "FK_DiemTichLuy_CUSTOMERS_KhachhangId",
                table: "DiemTichLuy");

            migrationBuilder.DropIndex(
                name: "IX_DiemTichLuy_HoadonId",
                table: "DiemTichLuy");

            migrationBuilder.DropIndex(
                name: "IX_DiemTichLuy_KhachhangId",
                table: "DiemTichLuy");

            migrationBuilder.DropColumn(
                name: "HoadonId",
                table: "DiemTichLuy");

            migrationBuilder.DropColumn(
                name: "KhachhangId",
                table: "DiemTichLuy");

            migrationBuilder.AddColumn<double>(
                name: "Diem",
                table: "Quyen",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "Quyen",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiamGia",
                table: "Quyen",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IdKhachHang",
                table: "DiemTichLuy",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IdHoaDon",
                table: "DiemTichLuy",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Diem",
                table: "DiemTichLuy",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_DiemTichLuy_IdHoaDon",
                table: "DiemTichLuy",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_DiemTichLuy_IdKhachHang",
                table: "DiemTichLuy",
                column: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_DiemTichLuy_CARTS_IdHoaDon",
                table: "DiemTichLuy",
                column: "IdHoaDon",
                principalTable: "CARTS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiemTichLuy_CUSTOMERS_IdKhachHang",
                table: "DiemTichLuy",
                column: "IdKhachHang",
                principalTable: "CUSTOMERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiemTichLuy_CARTS_IdHoaDon",
                table: "DiemTichLuy");

            migrationBuilder.DropForeignKey(
                name: "FK_DiemTichLuy_CUSTOMERS_IdKhachHang",
                table: "DiemTichLuy");

            migrationBuilder.DropIndex(
                name: "IX_DiemTichLuy_IdHoaDon",
                table: "DiemTichLuy");

            migrationBuilder.DropIndex(
                name: "IX_DiemTichLuy_IdKhachHang",
                table: "DiemTichLuy");

            migrationBuilder.DropColumn(
                name: "Diem",
                table: "Quyen");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "Quyen");

            migrationBuilder.DropColumn(
                name: "GiamGia",
                table: "Quyen");

            migrationBuilder.AlterColumn<int>(
                name: "IdKhachHang",
                table: "DiemTichLuy",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdHoaDon",
                table: "DiemTichLuy",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Diem",
                table: "DiemTichLuy",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "HoadonId",
                table: "DiemTichLuy",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KhachhangId",
                table: "DiemTichLuy",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiemTichLuy_HoadonId",
                table: "DiemTichLuy",
                column: "HoadonId");

            migrationBuilder.CreateIndex(
                name: "IX_DiemTichLuy_KhachhangId",
                table: "DiemTichLuy",
                column: "KhachhangId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiemTichLuy_CARTS_HoadonId",
                table: "DiemTichLuy",
                column: "HoadonId",
                principalTable: "CARTS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiemTichLuy_CUSTOMERS_KhachhangId",
                table: "DiemTichLuy",
                column: "KhachhangId",
                principalTable: "CUSTOMERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
