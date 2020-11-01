using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CATEGORIES",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryyId",
                table: "CATEGORIES",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CATEGORIES",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "CATEGORIES",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIES_CategoryyId",
                table: "CATEGORIES",
                column: "CategoryyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_CategoryyId",
                table: "CATEGORIES",
                column: "CategoryyId",
                principalTable: "CATEGORIES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_CategoryyId",
                table: "CATEGORIES");

            migrationBuilder.DropIndex(
                name: "IX_CATEGORIES_CategoryyId",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "CategoryyId",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CATEGORIES");
        }
    }
}
