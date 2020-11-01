using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_ParentId",
                table: "CATEGORIES");

            migrationBuilder.DropIndex(
                name: "IX_CATEGORIES_ParentId",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CATEGORIES");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "CATEGORIES",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_CategoryId",
                table: "CATEGORIES");

            migrationBuilder.DropIndex(
                name: "IX_CATEGORIES_CategoryId",
                table: "CATEGORIES");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "CATEGORIES",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "CATEGORIES",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIES_ParentId",
                table: "CATEGORIES",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_ParentId",
                table: "CATEGORIES",
                column: "ParentId",
                principalTable: "CATEGORIES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
