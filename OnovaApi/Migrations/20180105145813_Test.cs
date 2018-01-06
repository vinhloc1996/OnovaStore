using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnovaApi.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnonymousCustomer",
                columns: table => new
                {
                    AnonymousCustomerID = table.Column<string>(nullable: false),
                    IPAddress = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    LastUpdateDate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousCustomer", x => x.AnonymousCustomerID);
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
                name: "ProductStatus",
                columns: table => new
                {
                    ProductStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    StatusDescription = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    StatusName = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatus", x => x.ProductStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
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
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
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
                name: "RoleClaim",
                columns: table => new
                {
                    RoleClaimID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.RoleClaimID);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
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
                name: "UserClaim",
                columns: table => new
                {
                    UserClaimID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.UserClaimID);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    FullName = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    Gender = table.Column<bool>(nullable: true),
                    Picture = table.Column<string>(unicode: false, maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserProfile_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    AnonymouseCustomerID = table.Column<string>(maxLength: 450, nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserStatusID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_AnonymousCustomer_AnonymousCustomerID",
                        column: x => x.AnonymouseCustomerID,
                        principalTable: "AnonymousCustomer",
                        principalColumn: "AnonymousCustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "UserProfile",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_UserStatusID",
                        column: x => x.UserStatusID,
                        principalTable: "UserStatus",
                        principalColumn: "UserStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffID = table.Column<string>(nullable: false),
                    AddBy = table.Column<string>(maxLength: 450, nullable: true),
                    AddDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Address = table.Column<string>(maxLength: 256, nullable: false),
                    Phone = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Salary = table.Column<double>(nullable: false),
                    UserStatusID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK_Staff_StaffManagerID",
                        column: x => x.AddBy,
                        principalTable: "Staff",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_StaffID",
                        column: x => x.StaffID,
                        principalTable: "UserProfile",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_UserStatusID",
                        column: x => x.UserStatusID,
                        principalTable: "UserStatus",
                        principalColumn: "UserStatusID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "ShippingInfo",
                columns: table => new
                {
                    ShippingInfoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLine1 = table.Column<string>(maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(maxLength: 255, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    CustomerID = table.Column<string>(maxLength: 450, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    FullName = table.Column<string>(maxLength: 256, nullable: true),
                    Phone = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Zip = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingInfo", x => x.ShippingInfoID);
                    table.ForeignKey(
                        name: "FK_ShippingInfo_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralImage",
                columns: table => new
                {
                    GeneralImageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    StaffID = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralImage", x => x.GeneralImageID);
                    table.ForeignKey(
                        name: "FK_GeneralImage_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staff",
                        principalColumn: "StaffID",
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
                name: "Brand",
                columns: table => new
                {
                    BrandID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLine1 = table.Column<string>(maxLength: 255, nullable: false),
                    AddressLine2 = table.Column<string>(maxLength: 255, nullable: true),
                    BrandImage = table.Column<int>(nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    ContactEmail = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    ContactName = table.Column<string>(maxLength: 255, nullable: true),
                    ContactPhone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    ContactTitle = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Slug = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    Zip = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandID);
                    table.ForeignKey(
                        name: "FK_Brand_GeneralImageID",
                        column: x => x.BrandImage,
                        principalTable: "GeneralImage",
                        principalColumn: "GeneralImageID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryCode = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    CategoryImage = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ParentCategoryID = table.Column<int>(nullable: true),
                    Slug = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    TotalProduct = table.Column<int>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Category_GeneralImageID",
                        column: x => x.CategoryImage,
                        principalTable: "GeneralImage",
                        principalColumn: "GeneralImageID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_CategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdateDate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    PercentOff = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    PromotionCode = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    PromotionDescription = table.Column<string>(type: "ntext", nullable: true),
                    PromotionImage = table.Column<int>(nullable: true),
                    PromotionName = table.Column<string>(maxLength: 255, nullable: false),
                    PromotionStatus = table.Column<string>(maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TargetApply = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK_Promotion_GeneralImageID",
                        column: x => x.PromotionImage,
                        principalTable: "GeneralImage",
                        principalColumn: "GeneralImageID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BrandID = table.Column<int>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true),
                    CurrentQuantity = table.Column<int>(nullable: false),
                    DisplayPrice = table.Column<double>(nullable: false),
                    LastUpdateDate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    MaximumQuantity = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    ParentProductID = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    ProductLongDesc = table.Column<string>(type: "text", nullable: false),
                    ProductShortDesc = table.Column<string>(maxLength: 255, nullable: false),
                    ProductStatusID = table.Column<int>(nullable: true),
                    ProductThumbImage = table.Column<int>(nullable: true),
                    Rating = table.Column<float>(nullable: true, defaultValueSql: "((0))"),
                    RealPrice = table.Column<double>(nullable: false),
                    Slug = table.Column<string>(unicode: false, maxLength: 400, nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: true),
                    WishCounting = table.Column<int>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_ProductID",
                        column: x => x.ParentProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_ProductStatusID",
                        column: x => x.ProductStatusID,
                        principalTable: "ProductStatus",
                        principalColumn: "ProductStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_ProductThumbImage",
                        column: x => x.ProductThumbImage,
                        principalTable: "GeneralImage",
                        principalColumn: "GeneralImageID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnonymousCustomerCart",
                columns: table => new
                {
                    AnonymousCustomerID = table.Column<string>(nullable: false),
                    AnonymousCustomerCartID = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DisplayPrice = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    LastUpdate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    PriceDiscount = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    PromotionID = table.Column<int>(nullable: true),
                    Tax = table.Column<double>(nullable: true, defaultValueSql: "((0.15))"),
                    TotalPrice = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    TotalQuantity = table.Column<int>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousCustomerCart", x => x.AnonymousCustomerID);
                    table.UniqueConstraint("AK_AnonymousCustomerCart_AnonymousCustomerCartID", x => x.AnonymousCustomerCartID);
                    table.ForeignKey(
                        name: "FK_AnonymousCustomerCart_CustomerID",
                        column: x => x.AnonymousCustomerID,
                        principalTable: "AnonymousCustomer",
                        principalColumn: "AnonymousCustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnonymousCustomerCart_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCart",
                columns: table => new
                {
                    CustomerID = table.Column<string>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CustomerCartID = table.Column<int>(nullable: false),
                    DisplayPrice = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    LastUpdate = table.Column<byte[]>(rowVersion: true, nullable: false),
                    PriceDiscount = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    PromotionID = table.Column<int>(nullable: true),
                    Tax = table.Column<double>(nullable: true, defaultValueSql: "((0.15))"),
                    TotalPrice = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    TotalQuantity = table.Column<int>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCart", x => x.CustomerID);
                    table.UniqueConstraint("AK_CustomerCart_CustomerCartID", x => x.CustomerCartID);
                    table.ForeignKey(
                        name: "FK_CustomerCart_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerCart_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromotionBrand",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false),
                    BrandID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionBrand", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK_PromotionBrand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionBrand_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromotionCategory",
                columns: table => new
                {
                    PromotionID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCategory", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK_PromotionCategory_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionCategory_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
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

            migrationBuilder.CreateTable(
                name: "ProductNotification",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 254, nullable: false),
                    NotifyStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNotification", x => new { x.ProductID, x.Email });
                    table.ForeignKey(
                        name: "FK_ProductNotification_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
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
                name: "Review",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CustomerID = table.Column<string>(maxLength: 450, nullable: true),
                    IsBought = table.Column<bool>(nullable: false),
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
                name: "AnonymousCustomerCartDetail",
                columns: table => new
                {
                    AnonymousCustomerCartID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    DisplayPrice = table.Column<double>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    PriceDiscount = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    PromotionID = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousCustomerCartDetail", x => new { x.AnonymousCustomerCartID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_AnonymousCustomerCartDetail_CartID",
                        column: x => x.AnonymousCustomerCartID,
                        principalTable: "AnonymousCustomerCart",
                        principalColumn: "AnonymousCustomerCartID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnonymousCustomerCartDetail_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnonymousCustomerCartDetail_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCartDetail",
                columns: table => new
                {
                    CustomerCartID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    DisplayPrice = table.Column<double>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    PriceDiscount = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    PromotionID = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCartDetail", x => new { x.CustomerCartID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_CustomerCartDetail_CartID",
                        column: x => x.CustomerCartID,
                        principalTable: "CustomerCart",
                        principalColumn: "CustomerCartID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerCartDetail_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerCartDetail_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotion",
                        principalColumn: "PromotionID",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "ReviewConfirm",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false),
                    AssignStaffID = table.Column<string>(maxLength: 450, nullable: true),
                    IsApproved = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
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
                name: "UQ__Anonymou__71DF0F7EC0C6910C",
                table: "AnonymousCustomerCart",
                column: "AnonymousCustomerCartID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousCustomerCart_PromotionID",
                table: "AnonymousCustomerCart",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousCustomerCartDetail_ProductID",
                table: "AnonymousCustomerCartDetail",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_AnonymousCustomerCartDetail_PromotionID",
                table: "AnonymousCustomerCartDetail",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_BrandImage",
                table: "Brand",
                column: "BrandImage");

            migrationBuilder.CreateIndex(
                name: "UQ__Brand__FFA796CD6F298EB1",
                table: "Brand",
                column: "ContactEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Category__371BA9550BB1A02D",
                table: "Category",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryImage",
                table: "Category",
                column: "CategoryImage");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryID",
                table: "Category",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AnonymouseCustomerID",
                table: "Customer",
                column: "AnonymouseCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserStatusID",
                table: "Customer",
                column: "UserStatusID");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__EE78A1EDD2C7578E",
                table: "CustomerCart",
                column: "CustomerCartID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCart_PromotionID",
                table: "CustomerCart",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCartDetail_ProductID",
                table: "CustomerCartDetail",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCartDetail_PromotionID",
                table: "CustomerCartDetail",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotification_NotificationID",
                table: "CustomerNotification",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRecentView_ProductID",
                table: "CustomerRecentView",
                column: "ProductID");

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
                name: "IX_GeneralImage_StaffID",
                table: "GeneralImage",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_OptionDetail_OptionID",
                table: "OptionDetail",
                column: "OptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandID",
                table: "Product",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ParentProductID",
                table: "Product",
                column: "ParentProductID");

            migrationBuilder.CreateIndex(
                name: "UQ__Product__2F4E024F3574CE52",
                table: "Product",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductStatusID",
                table: "Product",
                column: "ProductStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductThumbImage",
                table: "Product",
                column: "ProductThumbImage");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_GeneralImageID",
                table: "ProductImage",
                column: "GeneralImageID");

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
                name: "UQ__ProductS__6A7B44FC2CACFE6F",
                table: "ProductStatus",
                column: "StatusCode",
                unique: true,
                filter: "[StatusCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Promotio__A617E4B6B4607E71",
                table: "Promotion",
                column: "PromotionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PromotionImage",
                table: "Promotion",
                column: "PromotionImage");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionBrand_BrandID",
                table: "PromotionBrand",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCategory_CategoryID",
                table: "PromotionCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionGroupProduct_ProductID",
                table: "PromotionGroupProduct",
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
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveForLater_ProductID",
                table: "SaveForLater",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingInfo_CustomerID",
                table: "ShippingInfo",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "UQ__Shipping__A9D10534837E440F",
                table: "ShippingInfo",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_AddBy",
                table: "Staff",
                column: "AddBy");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserStatusID",
                table: "Staff",
                column: "UserStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_StaffNotification_NotificationID",
                table: "StaffNotification",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribeStory_StoryID",
                table: "SubscribeStory",
                column: "StoryID");

            migrationBuilder.CreateIndex(
                name: "IX_UsefulReview_CustomerID",
                table: "UsefulReview",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_ProductID",
                table: "WishList",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnonymousCustomerCartDetail");

            migrationBuilder.DropTable(
                name: "CustomerCartDetail");

            migrationBuilder.DropTable(
                name: "CustomerNotification");

            migrationBuilder.DropTable(
                name: "CustomerPurchaseInfo");

            migrationBuilder.DropTable(
                name: "CustomerRecentView");

            migrationBuilder.DropTable(
                name: "ExcludeProductPromotionBrand");

            migrationBuilder.DropTable(
                name: "ExcludeProductPromotionCategory");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "ProductNotification");

            migrationBuilder.DropTable(
                name: "ProductOptionGroup");

            migrationBuilder.DropTable(
                name: "ProductPriceOff");

            migrationBuilder.DropTable(
                name: "ProductSprcificationValue");

            migrationBuilder.DropTable(
                name: "PromotionGroupProduct");

            migrationBuilder.DropTable(
                name: "ReviewConfirm");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "SaveForLater");

            migrationBuilder.DropTable(
                name: "ShippingInfo");

            migrationBuilder.DropTable(
                name: "StaffNotification");

            migrationBuilder.DropTable(
                name: "SubscribeStory");

            migrationBuilder.DropTable(
                name: "UsefulReview");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "AnonymousCustomerCart");

            migrationBuilder.DropTable(
                name: "CustomerCart");

            migrationBuilder.DropTable(
                name: "PromotionBrand");

            migrationBuilder.DropTable(
                name: "PromotionCategory");

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
                name: "Review");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "AnonymousCustomer");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ProductStatus");

            migrationBuilder.DropTable(
                name: "GeneralImage");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserStatus");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
