using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FIRST_NAME",
                table: "CUSTOMERS");

            migrationBuilder.DropColumn(
                name: "LAST_NAME",
                table: "CUSTOMERS");

            migrationBuilder.AddColumn<string>(
                name: "FULL_NAME",
                table: "CUSTOMERS",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FULL_NAME",
                table: "CUSTOMERS");

            migrationBuilder.AddColumn<string>(
                name: "FIRST_NAME",
                table: "CUSTOMERS",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LAST_NAME",
                table: "CUSTOMERS",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
