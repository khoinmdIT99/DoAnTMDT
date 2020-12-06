using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init01122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "ImportBill");

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "ImportBill",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "TienNo",
                table: "ImportBill",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TienNo",
                table: "ImportBill");

            migrationBuilder.AlterColumn<int>(
                name: "Payment",
                table: "ImportBill",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<int>(
                name: "DiscountValue",
                table: "ImportBill",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
