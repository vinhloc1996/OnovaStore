using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class UpdateNewProductDesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewProductLongDesc",
                table: "Product",
                newName: "ProductLongDesc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductLongDesc",
                table: "Product",
                newName: "NewProductLongDesc");
        }
    }
}
