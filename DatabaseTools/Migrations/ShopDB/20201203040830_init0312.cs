using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init0312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCTS_Suppliers_SupplierIdSupplier",
                table: "PRODUCTS");

            migrationBuilder.DropIndex(
                name: "IX_PRODUCTS_SupplierIdSupplier",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "SupplierIdSupplier",
                table: "PRODUCTS");

            migrationBuilder.AddColumn<int>(
                name: "IdWareHouse",
                table: "PRODUCTS",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WareHouse",
                columns: table => new
                {
                    IdWareHouse = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouse", x => x.IdWareHouse);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_IdSupplier",
                table: "PRODUCTS",
                column: "IdSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_IdWareHouse",
                table: "PRODUCTS",
                column: "IdWareHouse");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCTS_Suppliers_IdSupplier",
                table: "PRODUCTS",
                column: "IdSupplier",
                principalTable: "Suppliers",
                principalColumn: "IdSupplier",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCTS_WareHouse_IdWareHouse",
                table: "PRODUCTS",
                column: "IdWareHouse",
                principalTable: "WareHouse",
                principalColumn: "IdWareHouse",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCTS_Suppliers_IdSupplier",
                table: "PRODUCTS");

            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCTS_WareHouse_IdWareHouse",
                table: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "WareHouse");

            migrationBuilder.DropIndex(
                name: "IX_PRODUCTS_IdSupplier",
                table: "PRODUCTS");

            migrationBuilder.DropIndex(
                name: "IX_PRODUCTS_IdWareHouse",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "IdWareHouse",
                table: "PRODUCTS");

            migrationBuilder.AddColumn<int>(
                name: "SupplierIdSupplier",
                table: "PRODUCTS",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_SupplierIdSupplier",
                table: "PRODUCTS",
                column: "SupplierIdSupplier");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCTS_Suppliers_SupplierIdSupplier",
                table: "PRODUCTS",
                column: "SupplierIdSupplier",
                principalTable: "Suppliers",
                principalColumn: "IdSupplier",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
