using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class test7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RatedProducts_UserId",
                table: "RatedProducts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts");

            migrationBuilder.DropIndex(
                name: "IX_RatedProducts_UserId",
                table: "RatedProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatedProducts",
                table: "RatedProducts",
                column: "UserId");
        }
    }
}
