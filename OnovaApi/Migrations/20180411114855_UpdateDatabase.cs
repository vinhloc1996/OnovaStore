using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_AnonymousCustomerID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductID",
                table: "Product");

            migrationBuilder.DropTable(
                name: "CustomerNotification");

            migrationBuilder.DropTable(
                name: "ExcludeProductPromotionBrand");

            migrationBuilder.DropTable(
                name: "ExcludeProductPromotionCategory");

            migrationBuilder.DropTable(
                name: "ProductOptionGroup");

            migrationBuilder.DropTable(
                name: "ProductPriceOff");

            migrationBuilder.DropTable(
                name: "ProductSprcificationValue");

            migrationBuilder.DropTable(
                name: "PromotionGroupProduct");

            migrationBuilder.DropTable(
                name: "StaffNotification");

            migrationBuilder.DropTable(
                name: "SubscribeStory");

            migrationBuilder.DropTable(
                name: "OptionDetail");

            migrationBuilder.DropTable(
                name: "ProductSpecification");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "Subscriber");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Product_ParentProductID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Customer_AnonymouseCustomerID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ParentProductID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "AnonymouseCustomerID",
                table: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentProductID",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnonymouseCustomerID",
                table: "Customer",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExcludeProductPromotionBrand",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false),
                    BrandID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludeProductPromotionBrand", x => new { x.PromotionID, x.BrandID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_ExcludeProductPromotionBrand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExcludeProductPromotionBrand_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExcludeProductPromotionBrand_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "PromotionBrand",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcludeProductPromotionCategory",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludeProductPromotionCategory", x => new { x.PromotionID, x.CategoryID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_ExcludeProductPromotionCategory_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExcludeProductPromotionCategory_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExcludeProductPromotionCategory_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "PromotionCategory",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationDescription = table.Column<string>(type: "ntext", nullable: true),
                    NotificationName = table.Column<string>(maxLength: 100, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    OptionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OptionName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.OptionID);
                });

            migrationBuilder.CreateTable(
                name: "ProductPriceOff",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PercentOff = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    PriceOff = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPriceOff", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_ProductPriceOff_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecification",
                columns: table => new
                {
                    ProductSpecificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductSpecificationName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecification", x => x.ProductSpecificationID);
                });

            migrationBuilder.CreateTable(
                name: "PromotionGroupProduct",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionGroupProduct", x => new { x.PromotionID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_PromotionGroupProduct_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionGroupProduct_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    StoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastUpdateDate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    StoryDescription = table.Column<string>(type: "ntext", nullable: false),
                    StoryTitle = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.StoryID);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber",
                columns: table => new
                {
                    SubscribeEmail = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    LastUpdateDate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    StillSubscribe = table.Column<bool>(nullable: false),
                    SubscribedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UnsubscribeToken = table.Column<string>(unicode: false, maxLength: 100, nullable: true, defaultValueSql: "('')"),
                    UnsubscribeTokenExpire = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.SubscribeEmail);
                });

            migrationBuilder.CreateTable(
                name: "CustomerNotification",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    NotificationID = table.Column<int>(nullable: false),
                    LastUpdate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    NotifyStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotification", x => new { x.CustomerID, x.NotificationID });
                    table.ForeignKey(
                        name: "FK_CustomerNotification_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerNotification_NotificationID",
                        column: x => x.NotificationID,
                        principalTable: "Notification",
                        principalColumn: "NotificationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffNotification",
                columns: table => new
                {
                    StaffID = table.Column<string>(nullable: false),
                    NotificationID = table.Column<int>(nullable: false),
                    LastUpdate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    NotifyStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffNotification", x => new { x.StaffID, x.NotificationID });
                    table.ForeignKey(
                        name: "FK_StaffNotification_NotificationID",
                        column: x => x.NotificationID,
                        principalTable: "Notification",
                        principalColumn: "NotificationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffNotification_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staff",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OptionDetail",
                columns: table => new
                {
                    OptionDetailID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OptionDetailName = table.Column<string>(maxLength: 100, nullable: false),
                    OptionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionDetail", x => x.OptionDetailID);
                    table.ForeignKey(
                        name: "FK_OptionDetail_OptionID",
                        column: x => x.OptionID,
                        principalTable: "Option",
                        principalColumn: "OptionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSprcificationValue",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    ProductSpecificationID = table.Column<int>(nullable: false),
                    ProductSpecificationValue = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSprcificationValue", x => new { x.ProductID, x.ProductSpecificationID });
                    table.ForeignKey(
                        name: "FK_ProductSprcificationValue_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSprcificationValue_ProductSpecificationID",
                        column: x => x.ProductSpecificationID,
                        principalTable: "ProductSpecification",
                        principalColumn: "ProductSpecificationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscribeStory",
                columns: table => new
                {
                    SubscribeEmail = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    StoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribeStory", x => new { x.SubscribeEmail, x.StoryID });
                    table.ForeignKey(
                        name: "FK_SubscribeStory_StoryID",
                        column: x => x.StoryID,
                        principalTable: "Story",
                        principalColumn: "StoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscribeStory_SubcriberEmail",
                        column: x => x.SubscribeEmail,
                        principalTable: "Subscriber",
                        principalColumn: "SubscribeEmail",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionGroup",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    OptionID = table.Column<int>(nullable: false),
                    OptionDetailID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionGroup", x => new { x.ProductID, x.OptionID, x.OptionDetailID });
                    table.ForeignKey(
                        name: "FK_ProductOptionGroup_OptionDetailID",
                        column: x => x.OptionDetailID,
                        principalTable: "OptionDetail",
                        principalColumn: "OptionDetailID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductOptionGroup_OptionID",
                        column: x => x.OptionID,
                        principalTable: "Option",
                        principalColumn: "OptionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductOptionGroup_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ParentProductID",
                table: "Product",
                column: "ParentProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AnonymouseCustomerID",
                table: "Customer",
                column: "AnonymouseCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotification_NotificationID",
                table: "CustomerNotification",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeProductPromotionBrand_BrandID",
                table: "ExcludeProductPromotionBrand",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeProductPromotionBrand_ProductID",
                table: "ExcludeProductPromotionBrand",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeProductPromotionCategory_CategoryID",
                table: "ExcludeProductPromotionCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeProductPromotionCategory_ProductID",
                table: "ExcludeProductPromotionCategory",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OptionDetail_OptionID",
                table: "OptionDetail",
                column: "OptionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionGroup_OptionDetailID",
                table: "ProductOptionGroup",
                column: "OptionDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionGroup_OptionID",
                table: "ProductOptionGroup",
                column: "OptionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSprcificationValue_ProductSpecificationID",
                table: "ProductSprcificationValue",
                column: "ProductSpecificationID");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionGroupProduct_ProductID",
                table: "PromotionGroupProduct",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_StaffNotification_NotificationID",
                table: "StaffNotification",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribeStory_StoryID",
                table: "SubscribeStory",
                column: "StoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_AnonymousCustomerID",
                table: "Customer",
                column: "AnonymouseCustomerID",
                principalTable: "AnonymousCustomer",
                principalColumn: "AnonymousCustomerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductID",
                table: "Product",
                column: "ParentProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
