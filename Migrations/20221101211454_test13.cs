using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class test13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViewedProducts_Id",
                table: "ViewedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts");

            migrationBuilder.DropIndex(
                name: "IX_RatedProducts_ProductId",
                table: "RatedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts",
                columns: new[] { "ProductId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ViewedProducts_Id",
                table: "ViewedProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RatedProducts_ProductId",
                table: "RatedProducts",
                column: "ProductId");
        }
    }
}
