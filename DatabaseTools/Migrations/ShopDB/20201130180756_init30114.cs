using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init30114 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ImportBill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ImportBill",
                type: "money",
                nullable: true);
        }
    }
}
