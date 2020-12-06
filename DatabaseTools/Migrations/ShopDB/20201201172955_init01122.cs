using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init01122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateCreated",
                table: "ImportBill",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ImportBill",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
