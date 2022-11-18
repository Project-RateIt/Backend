using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class activatecodes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__activateCodes",
                table: "_activateCodes");

            migrationBuilder.RenameTable(
                name: "_activateCodes",
                newName: "ActivateCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ActivateCodes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ActivateCodes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivateCodes",
                table: "ActivateCodes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivateCodes_Id",
                table: "ActivateCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivateCodes_Users_UserId",
                table: "ActivateCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivateCodes_Users_UserId",
                table: "ActivateCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivateCodes",
                table: "ActivateCodes");

            migrationBuilder.DropIndex(
                name: "IX_ActivateCodes_Id",
                table: "ActivateCodes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ActivateCodes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ActivateCodes");

            migrationBuilder.RenameTable(
                name: "ActivateCodes",
                newName: "_activateCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK__activateCodes",
                table: "_activateCodes",
                column: "Id");
        }
    }
}
