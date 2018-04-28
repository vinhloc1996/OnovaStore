using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class UpdateProductDesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductLongDesc",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "NewProductLongDesc",
                table: "Product",
                type: "ntext",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewProductLongDesc",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ProductLongDesc",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
