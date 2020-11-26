using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class newversion112620204 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CARTS_CUSTOMERS_CUSTOMER_ID",
                table: "CARTS");

            migrationBuilder.DropIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS");

            migrationBuilder.AlterColumn<string>(
                name: "CUSTOMER_ID",
                table: "CARTS",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID",
                unique: true,
                filter: "[CUSTOMER_ID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CARTS_CUSTOMERS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID",
                principalTable: "CUSTOMERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CARTS_CUSTOMERS_CUSTOMER_ID",
                table: "CARTS");

            migrationBuilder.DropIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS");

            migrationBuilder.AlterColumn<string>(
                name: "CUSTOMER_ID",
                table: "CARTS",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CARTS_CUSTOMERS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID",
                principalTable: "CUSTOMERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
