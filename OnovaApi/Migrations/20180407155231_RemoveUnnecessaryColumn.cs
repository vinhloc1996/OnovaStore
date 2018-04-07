using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class RemoveUnnecessaryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__Product__2F4E024F48BD80E6",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "IsHide",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHide",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHide",
                table: "Brand",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHide",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsHide",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsHide",
                table: "Brand");

            migrationBuilder.AddColumn<byte[]>(
                name: "LastUpdateDate",
                table: "Product",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "Product",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "UQ__Product__2F4E024F48BD80E6",
                table: "Product",
                column: "ProductCode",
                unique: true);
        }
    }
}
