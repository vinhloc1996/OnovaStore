using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class FinalDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_UserStatusID",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "CustomerPurchaseInfo");

            migrationBuilder.DropTable(
                name: "CustomerRecentView");

            migrationBuilder.DropTable(
                name: "ReviewConfirm");

            migrationBuilder.DropTable(
                name: "SaveForLater");

            migrationBuilder.DropTable(
                name: "UsefulReview");

            migrationBuilder.DropTable(
                name: "UserStatus");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Staff_UserStatusID",
                table: "Staff");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerPurchaseInfo",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    TotalMoneySpend = table.Column<double>(nullable: false, defaultValueSql: "((0))"),
                    TotalQuantityOfPurchasedProduct = table.Column<int>(nullable: false, defaultValueSql: "((0))"),
                    TotalSuccessOrder = table.Column<int>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPurchaseInfo", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_CustomerPurchaseInfo_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRecentView",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRecentView", x => new { x.CustomerID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_CustomerRecentView_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerRecentView_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CustomerID = table.Column<string>(maxLength: 450, nullable: true),
                    IsBought = table.Column<bool>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    Rating = table.Column<byte>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReplyReviewID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    UsefulCounting = table.Column<int>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Review_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_ReviewID",
                        column: x => x.ReplyReviewID,
                        principalTable: "Review",
                        principalColumn: "ReviewID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaveForLater",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveForLater", x => new { x.CustomerID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_SaveForLater_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaveForLater_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserStatus",
                columns: table => new
                {
                    UserStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatus", x => x.UserStatusID);
                });

            migrationBuilder.CreateTable(
                name: "WishList",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishList", x => new { x.CustomerID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_WishList_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WishList_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewConfirm",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false),
                    AssignStaffID = table.Column<string>(maxLength: 450, nullable: true),
                    IsApproved = table.Column<bool>(nullable: true),
                    LastUpdateDate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    StaffComment = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewConfirm", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_ReviewConfirm_AssignStaffID",
                        column: x => x.AssignStaffID,
                        principalTable: "Staff",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewConfirm_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "Review",
                        principalColumn: "ReviewID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsefulReview",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsefulReview", x => new { x.ReviewID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_UsefulReview_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsefulReview_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "Review",
                        principalColumn: "ReviewID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserStatusID",
                table: "Staff",
                column: "UserStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecentView_ProductID",
                table: "CustomerRecentView",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CustomerID",
                table: "Review",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProductID",
                table: "Review",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReplyReviewID",
                table: "Review",
                column: "ReplyReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewConfirm_AssignStaffID",
                table: "ReviewConfirm",
                column: "AssignStaffID");

            migrationBuilder.CreateIndex(
                name: "IX_SaveForLater_ProductID",
                table: "SaveForLater",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_UsefulReview_CustomerID",
                table: "UsefulReview",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_ProductID",
                table: "WishList",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_UserStatusID",
                table: "Staff",
                column: "UserStatusID",
                principalTable: "UserStatus",
                principalColumn: "UserStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
