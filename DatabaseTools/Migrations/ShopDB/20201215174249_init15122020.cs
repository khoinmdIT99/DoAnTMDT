using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init15122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "CUSTOMERS",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Quyen",
                columns: table => new
                {
                    MaQuyen = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuyen = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quyen", x => x.MaQuyen);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    AdminId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_CUSTOMERS_AdminId",
                        column: x => x.AdminId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhanQuyen",
                columns: table => new
                {
                    MaQuyen = table.Column<int>(nullable: false),
                    MaTaiKhoan = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanQuyen", x => new { x.MaQuyen, x.MaTaiKhoan });
                    table.ForeignKey(
                        name: "FK_PhanQuyen_Quyen_MaQuyen",
                        column: x => x.MaQuyen,
                        principalTable: "Quyen",
                        principalColumn: "MaQuyen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhanQuyen_CUSTOMERS_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(maxLength: 500, nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    FromUserId = table.Column<string>(nullable: true),
                    ToRoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_CUSTOMERS_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Rooms_ToRoomId",
                        column: x => x.ToRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quyen",
                columns: new[] { "MaQuyen", "TenQuyen" },
                values: new object[,]
                {
                    { 1, "Quản trị" },
                    { 2, "Nhân viên" },
                    { 3, "Khách hàng" },
                    { 4, "Khách hàng thân thiết" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToRoomId",
                table: "Messages",
                column: "ToRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PhanQuyen_MaTaiKhoan",
                table: "PhanQuyen",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AdminId",
                table: "Rooms",
                column: "AdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PhanQuyen");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Quyen");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "CUSTOMERS");
        }
    }
}
