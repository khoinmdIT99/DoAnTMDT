using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_CategoryId",
                table: "CATEGORIES");

            migrationBuilder.DropIndex(
                name: "IX_CATEGORIES_CategoryId",
                table: "CATEGORIES");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "CATEGORIES",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "CATEGORIES",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIES_CategoryId",
                table: "CATEGORIES",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_CategoryId",
                table: "CATEGORIES",
                column: "CategoryId",
                principalTable: "CATEGORIES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
