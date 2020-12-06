using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init0212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ImportBill",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ImportBill",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ImportBill");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ImportBill");
        }
    }
}
