using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseTools.Migrations.ShopDB
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BLOG_CATEGORIES",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    SLUG = table.Column<string>(maxLength: 50, nullable: true),
                    BLOG_CATEGORY_NAME = table.Column<string>(maxLength: 255, nullable: true),
                    HIERARCHY_CODE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOG_CATEGORIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORIES",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    SLUG = table.Column<string>(maxLength: 50, nullable: true),
                    CATEGORY_NAME = table.Column<string>(maxLength: 255, nullable: true),
                    HIERARCHY_CODE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMERS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    FIRST_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    PASSWORD = table.Column<string>(maxLength: 255, nullable: true),
                    EMAIL = table.Column<string>(maxLength: 255, nullable: true),
                    PHONE_NO = table.Column<string>(maxLength: 20, nullable: true),
                    ADDRESS = table.Column<string>(nullable: true),
                    DISTRICT = table.Column<string>(nullable: true),
                    PROVINCE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MATERIALS",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    MATERIAL_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    NOTE = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIALS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_TYPES",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    TYPE_NAME = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_TYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PROVINCE",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROVINCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SHIPPING_ADDRESS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    FIRST_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_NAME = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    ADDRESS = table.Column<string>(nullable: true),
                    DISTRICT = table.Column<string>(nullable: true),
                    PROVINCE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPPING_ADDRESS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SHOP_SETTING",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    PAGE_NAME = table.Column<string>(nullable: false),
                    PAGE_TITLE = table.Column<string>(nullable: false),
                    PAGE_DESCRIPTION = table.Column<string>(nullable: true),
                    KEYWORD = table.Column<string>(nullable: true),
                    TAX_PERCENT = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHOP_SETTING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TAGS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    NAME = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAGS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BLOGS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    SLUG = table.Column<string>(nullable: false),
                    TITLE = table.Column<string>(maxLength: 255, nullable: false),
                    CONTENT = table.Column<string>(nullable: false),
                    BlogCategoryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOGS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BLOGS_BLOG_CATEGORIES_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BLOG_CATEGORIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMER_FEEDBACK",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    INDEX = table.Column<int>(nullable: false),
                    CUSTOMER_ID = table.Column<string>(nullable: true),
                    FEEDBACK = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER_FEEDBACK", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CUSTOMER_FEEDBACK_CUSTOMERS_CUSTOMER_ID",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    SLUG = table.Column<string>(maxLength: 50, nullable: false),
                    PRODUCT_CODE = table.Column<string>(maxLength: 50, nullable: false),
                    PRODUCT_NAME = table.Column<string>(maxLength: 255, nullable: false),
                    DESCRIPTION = table.Column<string>(nullable: true),
                    ProductTypeId = table.Column<string>(nullable: true),
                    MaterialId = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true),
                    PRICE_TYPE = table.Column<int>(nullable: true),
                    PRICE = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_CATEGORIES_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CATEGORIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_PRODUCT_TYPES_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "PRODUCT_TYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DISTRICT",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProvinceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISTRICT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DISTRICT_PROVINCE_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "PROVINCE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CARTS",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    CUSTOMER_ID = table.Column<string>(maxLength: 50, nullable: true),
                    SHIPPING_METHOD = table.Column<int>(nullable: false),
                    PAYMENT_METHOD = table.Column<int>(nullable: false),
                    TOTAL_PRICE = table.Column<long>(nullable: false),
                    DISCOUNT_PERCENT = table.Column<long>(nullable: false),
                    DISCOUNT = table.Column<long>(nullable: false),
                    TAX_PERCENT = table.Column<int>(nullable: false),
                    TAX = table.Column<long>(nullable: false),
                    SHIPPING_FEE = table.Column<long>(nullable: false),
                    TOTAL = table.Column<long>(nullable: false),
                    SHIPPING_ADDRESS_ID = table.Column<string>(maxLength: 50, nullable: true),
                    COMMENTS = table.Column<string>(nullable: true),
                    STATUS = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CARTS_CUSTOMERS_CUSTOMER_ID",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CARTS_SHIPPING_ADDRESS_SHIPPING_ADDRESS_ID",
                        column: x => x.SHIPPING_ADDRESS_ID,
                        principalTable: "SHIPPING_ADDRESS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SHOP_ADDRESS",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    SHOP_SETTING_ID = table.Column<string>(nullable: false),
                    IS_MAIN_ADDRESS = table.Column<bool>(nullable: false),
                    ADDRESS = table.Column<string>(nullable: false),
                    EMAIL = table.Column<string>(nullable: true),
                    HOTLINE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHOP_ADDRESS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SHOP_ADDRESS_SHOP_SETTING_SHOP_SETTING_ID",
                        column: x => x.SHOP_SETTING_ID,
                        principalTable: "SHOP_SETTING",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BLOG_IMAGE",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    URL = table.Column<string>(nullable: true),
                    THUMB_URL = table.Column<string>(nullable: true),
                    FILE_NAME = table.Column<string>(nullable: true),
                    FILE_SIZE = table.Column<long>(nullable: true),
                    FILE_TYPE = table.Column<string>(nullable: true),
                    BLOG_ID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOG_IMAGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BLOG_IMAGE_BLOGS_BLOG_ID",
                        column: x => x.BLOG_ID,
                        principalTable: "BLOGS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMER_FEEDBACK_IMAGE",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    URL = table.Column<string>(nullable: true),
                    THUMB_URL = table.Column<string>(nullable: true),
                    FILE_NAME = table.Column<string>(nullable: true),
                    FILE_SIZE = table.Column<long>(nullable: true),
                    FILE_TYPE = table.Column<string>(nullable: true),
                    CUSTOMER_FEEDBACK_ID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER_FEEDBACK_IMAGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CUSTOMER_FEEDBACK_IMAGE_CUSTOMER_FEEDBACK_CUSTOMER_FEEDBACK_ID",
                        column: x => x.CUSTOMER_FEEDBACK_ID,
                        principalTable: "CUSTOMER_FEEDBACK",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_IMAGE",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    URL = table.Column<string>(nullable: true),
                    THUMB_URL = table.Column<string>(nullable: true),
                    FILE_NAME = table.Column<string>(nullable: true),
                    FILE_SIZE = table.Column<long>(nullable: true),
                    FILE_TYPE = table.Column<string>(nullable: true),
                    PRODUCT_ID = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_IMAGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCT_IMAGE_PRODUCTS_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_REVIEW",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    TITLE = table.Column<string>(nullable: true),
                    REVIEW = table.Column<string>(nullable: true),
                    STAR = table.Column<int>(nullable: false),
                    APPROVED = table.Column<bool>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_REVIEW", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCT_REVIEW_CUSTOMERS_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCT_REVIEW_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_TAG",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    TAG_ID = table.Column<string>(maxLength: 50, nullable: false),
                    PRODUCT_ID = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_TAG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCT_TAG_PRODUCTS_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRODUCT_TAG_TAGS_TAG_ID",
                        column: x => x.TAG_ID,
                        principalTable: "TAGS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CART_PRODUCT",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 50, nullable: false),
                    CREATE_AT = table.Column<DateTime>(nullable: true),
                    LAST_UPDATE_AT = table.Column<DateTime>(nullable: true),
                    CREATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    LAST_UPDATE_BY = table.Column<string>(maxLength: 50, nullable: true),
                    CART_ID = table.Column<string>(maxLength: 50, nullable: true),
                    PRODUCT_ID = table.Column<string>(maxLength: 50, nullable: true),
                    PRICE_TYPE = table.Column<int>(nullable: true),
                    PRICE = table.Column<long>(nullable: true),
                    QUANTITY = table.Column<int>(nullable: false),
                    TOTAL = table.Column<long>(nullable: false),
                    BOUGHT = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CART_PRODUCT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CART_PRODUCT_CARTS_CART_ID",
                        column: x => x.CART_ID,
                        principalTable: "CARTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CART_PRODUCT_PRODUCTS_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PROVINCE",
                columns: new[] { "ID", "Name" },
                values: new object[] { "1", "Hà Nội" });

            migrationBuilder.InsertData(
                table: "PROVINCE",
                columns: new[] { "ID", "Name" },
                values: new object[] { "2", "Đà Nẵng" });

            migrationBuilder.InsertData(
                table: "PROVINCE",
                columns: new[] { "ID", "Name" },
                values: new object[] { "3", "Hồ Chí Minh" });

            migrationBuilder.InsertData(
                table: "DISTRICT",
                columns: new[] { "ID", "Name", "ProvinceId" },
                values: new object[,]
                {
                    { "1", "Ba Đình", "1" },
                    { "2", "Bắc Từ Liêm", "1" },
                    { "3", "Cầu Giấy", "1" },
                    { "4", "Đống Đa", "1" },
                    { "5", "Hà Đông", "1" },
                    { "6", "Hải Châu", "2" },
                    { "7", "Cẩm Lệ", "2" },
                    { "8", "Liên Chiểu", "2" },
                    { "9", "Ngũ Hành Sơn", "2" },
                    { "10", "Sơn Trà", "2" },
                    { "11", "Quận 1", "3" },
                    { "12", "Quận 2", "3" },
                    { "13", "Quận 3", "3" },
                    { "14", "Quận 4", "3" },
                    { "15", "Quận 5", "3" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BLOG_IMAGE_BLOG_ID",
                table: "BLOG_IMAGE",
                column: "BLOG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BLOGS_BlogCategoryId",
                table: "BLOGS",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CART_PRODUCT_CART_ID",
                table: "CART_PRODUCT",
                column: "CART_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CART_PRODUCT_PRODUCT_ID",
                table: "CART_PRODUCT",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_CUSTOMER_ID",
                table: "CARTS",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_SHIPPING_ADDRESS_ID",
                table: "CARTS",
                column: "SHIPPING_ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMER_FEEDBACK_CUSTOMER_ID",
                table: "CUSTOMER_FEEDBACK",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMER_FEEDBACK_IMAGE_CUSTOMER_FEEDBACK_ID",
                table: "CUSTOMER_FEEDBACK_IMAGE",
                column: "CUSTOMER_FEEDBACK_ID",
                unique: true,
                filter: "[CUSTOMER_FEEDBACK_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DISTRICT_ProvinceId",
                table: "DISTRICT",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_IMAGE_PRODUCT_ID",
                table: "PRODUCT_IMAGE",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_REVIEW_CustomerId",
                table: "PRODUCT_REVIEW",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_REVIEW_ProductId",
                table: "PRODUCT_REVIEW",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_TAG_PRODUCT_ID",
                table: "PRODUCT_TAG",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_TAG_TAG_ID",
                table: "PRODUCT_TAG",
                column: "TAG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_CategoryId",
                table: "PRODUCTS",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_MaterialId",
                table: "PRODUCTS",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_ProductTypeId",
                table: "PRODUCTS",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SHOP_ADDRESS_SHOP_SETTING_ID",
                table: "SHOP_ADDRESS",
                column: "SHOP_SETTING_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BLOG_IMAGE");

            migrationBuilder.DropTable(
                name: "CART_PRODUCT");

            migrationBuilder.DropTable(
                name: "CUSTOMER_FEEDBACK_IMAGE");

            migrationBuilder.DropTable(
                name: "DISTRICT");

            migrationBuilder.DropTable(
                name: "PRODUCT_IMAGE");

            migrationBuilder.DropTable(
                name: "PRODUCT_REVIEW");

            migrationBuilder.DropTable(
                name: "PRODUCT_TAG");

            migrationBuilder.DropTable(
                name: "SHOP_ADDRESS");

            migrationBuilder.DropTable(
                name: "BLOGS");

            migrationBuilder.DropTable(
                name: "CARTS");

            migrationBuilder.DropTable(
                name: "CUSTOMER_FEEDBACK");

            migrationBuilder.DropTable(
                name: "PROVINCE");

            migrationBuilder.DropTable(
                name: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "TAGS");

            migrationBuilder.DropTable(
                name: "SHOP_SETTING");

            migrationBuilder.DropTable(
                name: "BLOG_CATEGORIES");

            migrationBuilder.DropTable(
                name: "SHIPPING_ADDRESS");

            migrationBuilder.DropTable(
                name: "CUSTOMERS");

            migrationBuilder.DropTable(
                name: "CATEGORIES");

            migrationBuilder.DropTable(
                name: "MATERIALS");

            migrationBuilder.DropTable(
                name: "PRODUCT_TYPES");
        }
    }
}
