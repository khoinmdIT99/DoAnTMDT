using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiemTichLuy",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    IdKhachHang = table.Column<int>(nullable: false),
                    ThoiGian = table.Column<DateTime>(nullable: false),
                    Diem = table.Column<int>(nullable: false),
                    IdHoaDon = table.Column<int>(nullable: false),
                    HoadonId = table.Column<string>(nullable: true),
                    KhachhangId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiemTichLuy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiemTichLuy_CARTS_HoadonId",
                        column: x => x.HoadonId,
                        principalTable: "CARTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiemTichLuy_CUSTOMERS_KhachhangId",
                        column: x => x.KhachhangId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiemTichLuy_HoadonId",
                table: "DiemTichLuy",
                column: "HoadonId");

            migrationBuilder.CreateIndex(
                name: "IX_DiemTichLuy_KhachhangId",
                table: "DiemTichLuy",
                column: "KhachhangId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiemTichLuy");
        }
    }
}
