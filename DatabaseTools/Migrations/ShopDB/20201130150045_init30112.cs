using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init30112 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IDProduct",
                table: "ImportBillDetails",
                newName: "IdProduct");

            migrationBuilder.AlterColumn<string>(
                name: "IdProduct",
                table: "ImportBillDetails",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdProduct",
                table: "ImportBillDetails",
                newName: "IDProduct");

            migrationBuilder.AlterColumn<int>(
                name: "IDProduct",
                table: "ImportBillDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
