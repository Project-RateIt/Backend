using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class test9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotedProducts",
                table: "NotedProducts");

            migrationBuilder.DropIndex(
                name: "IX_NotedProducts_UserId",
                table: "NotedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotedProducts",
                table: "NotedProducts",
                columns: new[] { "UserId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NotedProducts",
                table: "NotedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotedProducts",
                table: "NotedProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NotedProducts_UserId",
                table: "NotedProducts",
                column: "UserId");
        }
    }
}
