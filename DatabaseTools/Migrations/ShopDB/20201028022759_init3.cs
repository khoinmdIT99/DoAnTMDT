using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CATEGORIES_CATEGORIES_CategoryyId",
                table: "CATEGORIES");

            migrationBuilder.DropIndex(
                name: "IX_CATEGORIES_CategoryyId",
                table: "CATEGORIES");

            migrationBuilder.DropColumn(
                name: "CategoryyId",
                table: "CATEGORIES");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "CATEGORIES",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CategoryyId",
                table: "CATEGORIES",
                type: "nvarchar(50)",
                nullable: true);

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
    }
}
