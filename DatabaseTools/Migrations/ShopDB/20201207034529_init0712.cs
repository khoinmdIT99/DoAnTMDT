using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init0712 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TinNhans",
                columns: table => new
                {
                    MaTinNhan = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGian = table.Column<DateTime>(nullable: false),
                    NoiDung = table.Column<string>(nullable: true),
                    MaDdh = table.Column<string>(nullable: true),
                    MaTaiKhoan = table.Column<string>(nullable: true),
                    SellerSeen = table.Column<bool>(nullable: true),
                    BuyerSeen = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhans", x => x.MaTinNhan);
                    table.ForeignKey(
                        name: "FK_TinNhans_CARTS_MaDdh",
                        column: x => x.MaDdh,
                        principalTable: "CARTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TinNhans_CUSTOMERS_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDdh = table.Column<string>(nullable: true),
                    NoiDung = table.Column<string>(nullable: true),
                    ThoiGian = table.Column<DateTime>(nullable: false),
                    MaTaiKhoan = table.Column<string>(nullable: true),
                    SellerSeen = table.Column<bool>(nullable: true),
                    BuyerSeen = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.MaThongBao);
                    table.ForeignKey(
                        name: "FK_ThongBaos_CARTS_MaDdh",
                        column: x => x.MaDdh,
                        principalTable: "CARTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaos_CUSTOMERS_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TranhChaps",
                columns: table => new
                {
                    MaTranhChap = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDDH = table.Column<string>(nullable: true),
                    LienHe = table.Column<string>(nullable: true),
                    NoiDung = table.Column<string>(nullable: true),
                    ThoiGian = table.Column<DateTime>(nullable: false),
                    TrangThai = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranhChaps", x => x.MaTranhChap);
                    table.ForeignKey(
                        name: "FK_TranhChaps_CARTS_MaDDH",
                        column: x => x.MaDDH,
                        principalTable: "CARTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_MaDdh",
                table: "TinNhans",
                column: "MaDdh");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_MaTaiKhoan",
                table: "TinNhans",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaos_MaDdh",
                table: "ThongBaos",
                column: "MaDdh");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaos_MaTaiKhoan",
                table: "ThongBaos",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_TranhChaps_MaDDH",
                table: "TranhChaps",
                column: "MaDDH");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TinNhans");

            migrationBuilder.DropTable(
                name: "ThongBaos");

            migrationBuilder.DropTable(
                name: "TranhChaps");
        }
    }
}
