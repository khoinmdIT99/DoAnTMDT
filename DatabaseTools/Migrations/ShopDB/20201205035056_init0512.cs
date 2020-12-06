using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init0512 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCTS_WareHouse_IdWareHouse",
                table: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "WareHouse");

            migrationBuilder.DropIndex(
                name: "IX_PRODUCTS_IdWareHouse",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "IdWareHouse",
                table: "PRODUCTS");

            migrationBuilder.AlterColumn<int>(
                name: "TOTAL",
                table: "CARTS",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdWareHouse",
                table: "PRODUCTS",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TOTAL",
                table: "CARTS",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "WareHouse",
                columns: table => new
                {
                    IdWareHouse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouse", x => x.IdWareHouse);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_IdWareHouse",
                table: "PRODUCTS",
                column: "IdWareHouse");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCTS_WareHouse_IdWareHouse",
                table: "PRODUCTS",
                column: "IdWareHouse",
                principalTable: "WareHouse",
                principalColumn: "IdWareHouse",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
