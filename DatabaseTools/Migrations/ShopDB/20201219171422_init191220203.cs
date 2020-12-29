using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init191220203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotThat",
                table: "CATEGORIES");

            migrationBuilder.AddColumn<string>(
                name: "NoiThat",
                table: "CATEGORIES",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoiThat",
                table: "CATEGORIES");

            migrationBuilder.AddColumn<string>(
                name: "NotThat",
                table: "CATEGORIES",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
