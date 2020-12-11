using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init06122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID",
                unique: true,
                filter: "[CUSTOMER_ID] IS NOT NULL");
        }
    }
}
