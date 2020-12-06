using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init30113 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportBill_Suppliers_SupplierIdSupplier",
                table: "ImportBill");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportBillDetails_ImportBill_ImportBillIdImport",
                table: "ImportBillDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportBillDetails_ImportBillIdImport",
                table: "ImportBillDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportBill_SupplierIdSupplier",
                table: "ImportBill");

            migrationBuilder.DropColumn(
                name: "ImportBillIdImport",
                table: "ImportBillDetails");

            migrationBuilder.DropColumn(
                name: "SupplierIdSupplier",
                table: "ImportBill");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBillDetails_IdImport",
                table: "ImportBillDetails",
                column: "IdImport");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBill_IdSupplier",
                table: "ImportBill",
                column: "IdSupplier");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBill_Suppliers_IdSupplier",
                table: "ImportBill",
                column: "IdSupplier",
                principalTable: "Suppliers",
                principalColumn: "IdSupplier",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBillDetails_ImportBill_IdImport",
                table: "ImportBillDetails",
                column: "IdImport",
                principalTable: "ImportBill",
                principalColumn: "IdImport",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportBill_Suppliers_IdSupplier",
                table: "ImportBill");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportBillDetails_ImportBill_IdImport",
                table: "ImportBillDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportBillDetails_IdImport",
                table: "ImportBillDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportBill_IdSupplier",
                table: "ImportBill");

            migrationBuilder.AddColumn<int>(
                name: "ImportBillIdImport",
                table: "ImportBillDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierIdSupplier",
                table: "ImportBill",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportBillDetails_ImportBillIdImport",
                table: "ImportBillDetails",
                column: "ImportBillIdImport");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBill_SupplierIdSupplier",
                table: "ImportBill",
                column: "SupplierIdSupplier");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBill_Suppliers_SupplierIdSupplier",
                table: "ImportBill",
                column: "SupplierIdSupplier",
                principalTable: "Suppliers",
                principalColumn: "IdSupplier",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBillDetails_ImportBill_ImportBillIdImport",
                table: "ImportBillDetails",
                column: "ImportBillIdImport",
                principalTable: "ImportBill",
                principalColumn: "IdImport",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
