using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    ResetPassKey = table.Column<string>(type: "text", nullable: true),
                    AddedProduct = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    HaveAvatar = table.Column<bool>(type: "boolean", nullable: false),
                    AccountType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_subcategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK__subcategories__categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Producer = table.Column<string>(type: "text", nullable: true),
                    Ean = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubcategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    RateSum = table.Column<int>(type: "integer", nullable: false),
                    RateCount = table.Column<int>(type: "integer", nullable: false),
                    Sponsor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products", x => x.Id);
                    table.ForeignKey(
                        name: "FK__products__categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__products__subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "_subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_notedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__notedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK__notedProducts__products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "_products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__notedProducts__users_UserId",
                        column: x => x.UserId,
                        principalTable: "_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_ratedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ratedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ratedProducts__products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "_products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ratedProducts__users_UserId",
                        column: x => x.UserId,
                        principalTable: "_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_viewedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__viewedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK__viewedProducts__products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "_products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__viewedProducts__users_UserId",
                        column: x => x.UserId,
                        principalTable: "_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__notedProducts_ProductId",
                table: "_notedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX__notedProducts_UserId",
                table: "_notedProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX__products_CategoryId",
                table: "_products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX__products_SubcategoryId",
                table: "_products",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX__ratedProducts_ProductId",
                table: "_ratedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX__ratedProducts_UserId",
                table: "_ratedProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX__subcategories_CategoryId",
                table: "_subcategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX__viewedProducts_ProductId",
                table: "_viewedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX__viewedProducts_UserId",
                table: "_viewedProducts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_notedProducts");

            migrationBuilder.DropTable(
                name: "_ratedProducts");

            migrationBuilder.DropTable(
                name: "_viewedProducts");

            migrationBuilder.DropTable(
                name: "_products");

            migrationBuilder.DropTable(
                name: "_users");

            migrationBuilder.DropTable(
                name: "_subcategories");

            migrationBuilder.DropTable(
                name: "_categories");
        }
    }
}
