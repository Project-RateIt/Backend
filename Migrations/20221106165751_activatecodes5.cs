using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class activatecodes5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivateCodes",
                table: "ActivateCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivateCodes",
                table: "ActivateCodes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ActivateCodes_UserId",
                table: "ActivateCodes",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivateCodes",
                table: "ActivateCodes");

            migrationBuilder.DropIndex(
                name: "IX_ActivateCodes_UserId",
                table: "ActivateCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivateCodes",
                table: "ActivateCodes",
                column: "UserId");
        }
    }
}
