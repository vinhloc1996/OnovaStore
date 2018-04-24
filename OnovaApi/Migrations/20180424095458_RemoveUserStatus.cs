using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class RemoveUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_UserStatusID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_UserStatusID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "ShippingInfo");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UserStatusID",
                table: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "ShippingInfo",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Order",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserStatusID",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserStatusID",
                table: "Customer",
                column: "UserStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_UserStatusID",
                table: "Customer",
                column: "UserStatusID",
                principalTable: "UserStatus",
                principalColumn: "UserStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
