using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rateit.Migrations
{
    public partial class test11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "ViewedProducts",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "ViewedProducts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
