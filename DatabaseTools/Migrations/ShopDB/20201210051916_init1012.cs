using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init1012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportBillDetails_ImportBill_IdImport",
                table: "ImportBillDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBillDetails_ImportBill_IdImport",
                table: "ImportBillDetails",
                column: "IdImport",
                principalTable: "ImportBill",
                principalColumn: "IdImport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportBillDetails_ImportBill_IdImport",
                table: "ImportBillDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBillDetails_ImportBill_IdImport",
                table: "ImportBillDetails",
                column: "IdImport",
                principalTable: "ImportBill",
                principalColumn: "IdImport",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
