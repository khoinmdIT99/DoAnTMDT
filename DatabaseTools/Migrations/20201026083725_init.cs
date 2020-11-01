using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MAIL_SETTING",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    SMTP_SERVER = table.Column<string>(nullable: true),
                    SMTP_PORT = table.Column<int>(nullable: true),
                    SMTP_USERNAME = table.Column<string>(nullable: true),
                    SMTP_PASSWORD = table.Column<string>(nullable: true),
                    SENDER_EMAIL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAIL_SETTING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MENUS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    ORDER = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(maxLength: 50, nullable: false),
                    DISPLAY_NAME = table.Column<string>(maxLength: 255, nullable: false),
                    ICON = table.Column<string>(maxLength: 255, nullable: true),
                    HIERARCHY_CODE = table.Column<string>(nullable: false),
                    CONTROLLER = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENUS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    ROLE_CODE = table.Column<string>(maxLength: 50, nullable: true),
                    ROLE_NAME = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SITE_SETTING",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    PAGE_TITLE = table.Column<string>(maxLength: 255, nullable: false),
                    PAGE_EMAIL = table.Column<string>(maxLength: 255, nullable: false),
                    DEFAULT_PAGE_SIZE = table.Column<int>(nullable: true),
                    PAGE_SIZE_OPTIONS = table.Column<string>(nullable: true),
                    SHOW_FOOTER = table.Column<bool>(nullable: false),
                    FOOTER_DATA = table.Column<string>(nullable: true),
                    ICON = table.Column<string>(nullable: true),
                    LOGO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SITE_SETTING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SSO_SETTING",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    ENABLE_GOOGLE_AUTH0 = table.Column<bool>(nullable: false),
                    GOOGLE_CLIENT_ID = table.Column<string>(nullable: true),
                    GOOGLE_CLIENT_SECRET = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSO_SETTING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    FULL_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    USER_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    PASSWORD = table.Column<string>(maxLength: 255, nullable: true),
                    EMAIL = table.Column<string>(maxLength: 255, nullable: true),
                    PHONE_NO = table.Column<string>(maxLength: 20, nullable: true),
                    DAY_OF_BIRTH = table.Column<DateTime>(nullable: true),
                    GENDER = table.Column<int>(nullable: true),
                    PROFILE_IMAGE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MENU_ROLE",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    MENU_ID = table.Column<string>(nullable: true),
                    ROLE_ID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_ROLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MENU_ROLE_MENUS_MENU_ID",
                        column: x => x.MENU_ID,
                        principalTable: "MENUS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_ROLE_ROLES_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLE",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    ROLE_ID = table.Column<string>(maxLength: 50, nullable: true),
                    USER_ID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USER_ROLE_ROLES_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USER_ROLE_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MENU_ROLE_MENU_ID",
                table: "MENU_ROLE",
                column: "MENU_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_ROLE_ROLE_ID",
                table: "MENU_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLE_ROLE_ID",
                table: "USER_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLE_USER_ID",
                table: "USER_ROLE",
                column: "USER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MAIL_SETTING");

            migrationBuilder.DropTable(
                name: "MENU_ROLE");

            migrationBuilder.DropTable(
                name: "SITE_SETTING");

            migrationBuilder.DropTable(
                name: "SSO_SETTING");

            migrationBuilder.DropTable(
                name: "USER_ROLE");

            migrationBuilder.DropTable(
                name: "MENUS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
