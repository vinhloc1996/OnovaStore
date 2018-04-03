using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class ChangeGeneralImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brand_GeneralImageID",
                table: "Brand");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_GeneralImageID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductThumbImage",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_GeneralImageID",
                table: "Promotion");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_PromotionImage",
                table: "Promotion");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductThumbImage",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Category_CategoryImage",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Brand_BrandImage",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "PromotionImage",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "ProductThumbImage",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "GeneralImage");

            migrationBuilder.DropColumn(
                name: "CategoryImage",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BrandImage",
                table: "Brand");

            //----

            migrationBuilder.DropPrimaryKey("PK_GeneralImage", "GeneralImage");

            migrationBuilder.DropColumn(
                name: "GeneralImageID",
                table: "GeneralImage");

            migrationBuilder.AddColumn<string>(
                name: "GeneralImageID",
                table: "GeneralImage");

//            migrationBuilder.AlterColumn<string>(
//                name: "GeneralImageID",
//                table: "GeneralImage",
//                nullable: false,
//                oldClrType: typeof(int))
//                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromotionImage",
                table: "Promotion",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductThumbImage",
                table: "Product",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GeneralImageID",
                table: "GeneralImage",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "GeneralImage",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CategoryImage",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandImage",
                table: "Brand",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    GeneralImageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => new { x.ProductID, x.GeneralImageID });
                    table.ForeignKey(
                        name: "FK_ProductImage_GeneralImageID",
                        column: x => x.GeneralImageID,
                        principalTable: "GeneralImage",
                        principalColumn: "GeneralImageID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductImage_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PromotionImage",
                table: "Promotion",
                column: "PromotionImage");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductThumbImage",
                table: "Product",
                column: "ProductThumbImage");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryImage",
                table: "Category",
                column: "CategoryImage");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_BrandImage",
                table: "Brand",
                column: "BrandImage");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_GeneralImageID",
                table: "ProductImage",
                column: "GeneralImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_GeneralImageID",
                table: "Brand",
                column: "BrandImage",
                principalTable: "GeneralImage",
                principalColumn: "GeneralImageID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_GeneralImageID",
                table: "Category",
                column: "CategoryImage",
                principalTable: "GeneralImage",
                principalColumn: "GeneralImageID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductThumbImage",
                table: "Product",
                column: "ProductThumbImage",
                principalTable: "GeneralImage",
                principalColumn: "GeneralImageID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_GeneralImageID",
                table: "Promotion",
                column: "PromotionImage",
                principalTable: "GeneralImage",
                principalColumn: "GeneralImageID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
