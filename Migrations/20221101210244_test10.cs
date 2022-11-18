using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class test10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ViewedProducts",
                table: "ViewedProducts");

            migrationBuilder.DropIndex(
                name: "IX_ViewedProducts_UserId",
                table: "ViewedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViewedProducts",
                table: "ViewedProducts",
                columns: new[] { "UserId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ViewedProducts",
                table: "ViewedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViewedProducts",
                table: "ViewedProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ViewedProducts_UserId",
                table: "ViewedProducts",
                column: "UserId");
        }
    }
}
