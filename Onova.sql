USE master
IF EXISTS(SELECT * FROM sys.databases WHERE [name]='Onova')
DROP DATABASE Onova
GO
CREATE DATABASE Onova
GO
USE Onova
GO

/*
	*** Change Log ***
	#1: Update NOT NULL for DEFAULT columns
	#2: Change Position NOT NULL TO BEHIND DEFAULT
	#3: Only Customer is allowed to have shipping info
	#4: Each type of customer have 1 cart table(Pros: When merge the anonymous customer to authenticated customer, the anonymous cart can easily merge into the normal cart)
	#5: Order should have only one table for both type of customers. Therefore, it should have specified address columns
	#6: Remove StillAnonymous column, becasue the anonymous customer only generated when user add the product to cart. After merging the authentication, 
		the anonymous cart will be merged to cart and it will be deleted. Afterward, the anonymous customer will be deleted to avoid the redundancy
	#7: Remove Customer EmailConfirmation column. Add test record for regular login function
	#8: Remove UserProfile and manually add relationship with AspNetUser Table in code first after generating the model and dbcontext
	#9: Remove Email in ShippingInfo, get email directly from User information
*/

/*-------------------- Start #User and its relate tables --------------------*/

CREATE TABLE AnonymousCustomer
(
	AnonymousCustomerID NVARCHAR(450) PRIMARY KEY,
	VisitDate DATETIME NOT NULL,
	LastUpdateDate TIMESTAMP NOT NULL,
	IPAddress VARCHAR(50) NOT NULL
)
GO
CREATE TABLE UserProfile
(
	UserID NVARCHAR(450),
	FullName VARCHAR(256),
	Picture VARCHAR(256),
	Gender BIT,
	DateOfBirth DATE,
	CONSTRAINT PK_UserProfile_UserID PRIMARY KEY (UserID)
)
GO
CREATE TABLE UserStatus
(
	UserStatusID INT IDENTITY(1,1) PRIMARY KEY,
	Code VARCHAR(50) NOT NULL,
	[Name] VARCHAR(100) NOT NULL
)
GO
CREATE TABLE Staff
(
	StaffID NVARCHAR(450) CONSTRAINT FK_Staff_StaffID REFERENCES UserProfile(UserID),
	AddDate DATETIME NOT NULL,
	AddBy NVARCHAR(450) CONSTRAINT FK_Staff_StaffManagerID REFERENCES Staff(StaffID),
	UserStatusID INT CONSTRAINT FK_Staff_UserStatusID REFERENCES UserStatus(UserStatusID),
	Salary FLOAT NOT NULL,
	[Address] NVARCHAR(256) NOT NULL,
	Phone VARCHAR(20) NOT NULL,
	CONSTRAINT PK_Staff_StaffID PRIMARY KEY (StaffID)
)
GO
CREATE TABLE Customer
(
	CustomerID NVARCHAR(450) CONSTRAINT FK_Customer_CustomerID REFERENCES UserProfile(UserID),
	UserStatusID INT CONSTRAINT FK_Customer_UserStatusID REFERENCES UserStatus(UserStatusID),
	JoinDate DATETIME NOT NULL,
	AnonymouseCustomerID NVARCHAR(450) CONSTRAINT FK_AnonymousCustomer_AnonymousCustomerID REFERENCES AnonymousCustomer(AnonymousCustomerID),
	CONSTRAINT PK_Customer_CustomerID PRIMARY KEY (CustomerID)
)
GO
CREATE TABLE ShippingInfo
(
	ShippingInfoID INT IDENTITY(1,1) PRIMARY KEY,
	FullName NVARCHAR(256),
	AddressLine1 NVARCHAR(255) NOT NULL,
	AddressLine2 NVARCHAR(255),
	City NVARCHAR(100) NOT NULL,
	Zip VARCHAR(50) NOT NULL,
	Phone VARCHAR(20) NOT NULL,
	CustomerID NVARCHAR(450) CONSTRAINT FK_ShippingInfo_CustomerID REFERENCES Customer(CustomerID)
)
GO
CREATE TABLE CustomerPurchaseInfo
(
	CustomerID NVARCHAR(450) CONSTRAINT FK_CustomerPurchaseInfo_CustomerID REFERENCES Customer(CustomerID),
	TotalSuccessOrder INT DEFAULT(0) NOT NULL,
	TotalMoneySpend FLOAT DEFAULT(0) NOT NULL,
	TotalQuantityOfPurchasedProduct INT DEFAULT(0) NOT NULL,
	CONSTRAINT PK_CustomerPurchaseInfo_CustomerID PRIMARY KEY(CustomerID)
)

/*-------------------- End #User and its relate tables --------------------*/

GO

/*-------------------- Start #Product and its relate tables --------------------*/

CREATE TABLE GeneralImage
(
	GeneralImageID INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(200),
	ImageURL TEXT NOT NULL,
	AddDate DATETIME NOT NULL,
	StaffID NVARCHAR(450) CONSTRAINT FK_GeneralImage_StaffID REFERENCES Staff(StaffID)
)
GO
CREATE TABLE Category
(
	CategoryID INT IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(100),
	CategoryCode VARCHAR(200) UNIQUE NOT NULL,
	Slug VARCHAR(200) NOT NULL,
	TotalProduct INT DEFAULT(0) NOT NULL,
	CategoryImage INT CONSTRAINT FK_Category_GeneralImageID REFERENCES GeneralImage(GeneralImageID),
	ParentCategoryID INT CONSTRAINT FK_Category_CategoryID REFERENCES Category(CategoryID)
)
GO
CREATE TABLE Brand
(
	BrandID INT IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(100),
	ContactName NVARCHAR(255),
	ContactTitle VARCHAR(20),
	ContactPhone VARCHAR(20),
	ContactEmail VARCHAR(254) UNIQUE NOT NULL,
	AddressLine1 NVARCHAR(255) NOT NULL,
	AddressLine2 NVARCHAR(255),
	BrandImage INT CONSTRAINT FK_Brand_GeneralImageID REFERENCES GeneralImage(GeneralImageID),
	City NVARCHAR(100) NOT NULL,
	Zip VARCHAR(50) NOT NULL,
	Slug VARCHAR(200) NOT NULL
)
GO
CREATE TABLE ProductStatus
(
	ProductStatusID INT IDENTITY(1,1) PRIMARY KEY,
	StatusCode VARCHAR(50) UNIQUE,
	StatusName VARCHAR(100),
	StatusDescription VARCHAR(500)
)
GO
CREATE TABLE Product
(
	ProductID INT IDENTITY(1,1) PRIMARY KEY,
	ProductCode VARCHAR(200) UNIQUE NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Weight] FLOAT,
	RealPrice FLOAT NOT NULL, /*Price from Brand*/
	DisplayPrice FLOAT NOT NULL, /*Price normal without discount display to customer*/
	ProductShortDesc NVARCHAR(255) NOT NULL,
	ProductLongDesc TEXT NOT NULL,
	ProductThumbImage INT CONSTRAINT FK_Product_ProductThumbImage REFERENCES GeneralImage(GeneralImageID),
	Slug VARCHAR(400) NOT NULL,
	CategoryID INT CONSTRAINT FK_Product_CategoryID REFERENCES Category(CategoryID),
	BrandID INT CONSTRAINT FK_Product_BrandID REFERENCES Brand(BrandID),
	TotalQuantity INT NOT NULL, /*The total quantity from lasted import time*/
	CurrentQuantity INT NOT NULL, /*The current quantity of the product*/
	MaximumQuantity INT NOT NULL, /*The maximum quantity which customer can buy in a order*/
	AddDate DATETIME NOT NULL,
	LastUpdateDate TIMESTAMP NOT NULL,
	Rating FLOAT(1) DEFAULT(0),
	WishCounting INT DEFAULT(0),
	ProductStatusID INT CONSTRAINT FK_Product_ProductStatusID REFERENCES ProductStatus(ProductStatusID),
	ParentProductID INT CONSTRAINT FK_Product_ProductID REFERENCES Product(ProductID)
)
GO
CREATE TABLE ProductNotification
(
	ProductID INT CONSTRAINT FK_ProductNotification_ProductID REFERENCES Product(ProductID),
	Email VARCHAR(254) NOT NULL,
	NotifyStatus BIT NOT NULL,
	CONSTRAINT PK_ProductNotification_ID PRIMARY KEY(ProductID, Email)
)
GO
CREATE TABLE ProductPriceOff
(
	ProductID INT CONSTRAINT FK_ProductPriceOff_ProductID REFERENCES Product(ProductID),
	PercentOff DECIMAL NOT NULL,
	PriceOff FLOAT NOT NULL, /*Auto calculate by backend*/
	StartDate DATETIME NOT NULL,
	EndDate DATETIME,
	CONSTRAINT PK_ProductPrice_ProductID PRIMARY KEY(ProductID)
)
GO
CREATE TABLE ProductImage
(
	ProductID INT CONSTRAINT FK_ProductImage_ProductID REFERENCES Product(ProductID),
	GeneralImageID INT CONSTRAINT FK_ProductImage_GeneralImageID REFERENCES GeneralImage(GeneralImageID),
	CONSTRAINT PK_ProductImage_ID PRIMARY KEY(ProductID, GeneralImageID)
)
GO
CREATE TABLE [Option]
(
	OptionID INT IDENTITY(1,1) PRIMARY KEY,
	OptionName NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE OptionDetail
(
	OptionDetailID INT IDENTITY(1,1) PRIMARY KEY,
	OptionDetailName NVARCHAR(100) NOT NULL,
	OptionID INT CONSTRAINT FK_OptionDetail_OptionID REFERENCES [Option](OptionID)
)
GO
CREATE TABLE ProductOptionGroup
(
	ProductID INT CONSTRAINT FK_ProductOptionGroup_ProductID REFERENCES Product(ProductID),
	OptionID INT CONSTRAINT FK_ProductOptionGroup_OptionID REFERENCES [Option](OptionID),
	OptionDetailID INT CONSTRAINT FK_ProductOptionGroup_OptionDetailID REFERENCES OptionDetail(OptionDetailID),
	CONSTRAINT PK_ProductOptionGroup_ID PRIMARY KEY (ProductID, OptionID, OptionDetailID)
)
GO
CREATE TABLE ProductSpecification
(
	ProductSpecificationID INT IDENTITY(1,1) PRIMARY KEY,
	ProductSpecificationName NVARCHAR(100) NOT NULL
)
GO
CREATE TABLE ProductSprcificationValue
(
	ProductID INT CONSTRAINT FK_ProductSprcificationValue_ProductID REFERENCES Product(ProductID),
	ProductSpecificationID INT CONSTRAINT FK_ProductSprcificationValue_ProductSpecificationID REFERENCES ProductSpecification(ProductSpecificationID),
	ProductSpecificationValue NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_ProductSpecificationValue_ID PRIMARY KEY (ProductID, ProductSpecificationID)
)
GO
CREATE TABLE CustomerRecentView
(
	CustomerID NVARCHAR(450) CONSTRAINT FK_CustomerRecentView_CustomerID REFERENCES Customer(CustomerID),
	ProductID INT CONSTRAINT FK_CustomerRecentView_ProductID REFERENCES Product(ProductID),
	CONSTRAINT PK_CustomerRecentView_ID PRIMARY KEY (CustomerID, ProductID)
)
GO
CREATE TRIGGER IncreaseProductInCategory ON Product
FOR INSERT
AS
	BEGIN
		DECLARE @CategoryID INT
		IF @@ROWCOUNT = 1
			BEGIN
				SELECT @CategoryID = CategoryID FROM inserted
				UPDATE Category SET TotalProduct += 1 WHERE CategoryID = @CategoryID
			END
		ELSE
			BEGIN
				RAISERROR('Error while adding product', 16, 1)
				ROLLBACK
			END
	END

DROP TRIGGER IncreaseProductInCategory

/*-------------------- End #Product and its relate tables --------------------*/

GO

/*-------------------- Start #Subsriber and its relate tables --------------------*/

CREATE TABLE Story
(
	StoryID INT IDENTITY(1,1) PRIMARY KEY,
	StoryTitle NVARCHAR(200) NOT NULL,
	StoryDescription NTEXT NOT NULL,
	LastUpdateDate TIMESTAMP NOT NULL
)
GO
CREATE TABLE Subscriber
(
	SubscribeEmail VARCHAR(254) PRIMARY KEY,
	StillSubscribe BIT NOT NULL,
	SubscribedDate DATETIME NOT NULL,
	LastUpdateDate TIMESTAMP NOT NULL,
	UnsubscribeToken VARCHAR(100) DEFAULT(''),
	UnsubscribeTokenExpire DATETIME
)
GO
CREATE TABLE SubscribeStory
(
	SubscribeEmail VARCHAR(254) CONSTRAINT FK_SubscribeStory_SubcriberEmail REFERENCES Subscriber(SubscribeEmail),
	StoryID INT CONSTRAINT FK_SubscribeStory_StoryID REFERENCES Story(StoryID),
	CONSTRAINT PK_SubscribeStory_ID PRIMARY KEY(SubscribeEmail, StoryID)

)

/*-------------------- End #Subsriber and its relate tables --------------------*/

GO

/*-------------------- Start #Promotion and its relate tables --------------------*/

CREATE TABLE Promotion
(
	PromotionID INT IDENTITY(1,1) PRIMARY KEY,
	PromotionStatus NVARCHAR(50) NOT NULL,
	PromotionName NVARCHAR(255) NOT NULL,
	PromotionDescription NTEXT,
	PromotionCode VARCHAR(50) NOT NULL UNIQUE,
	PromotionImage INT CONSTRAINT FK_Promotion_GeneralImageID REFERENCES GeneralImage(GeneralImageID),
	TargetApply NVARCHAR(50) NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME,
	PercentOff DECIMAL NOT NULL,
	LastUpdateDate TIMESTAMP
)
GO
CREATE TABLE PromotionGroupProduct
(
	PromotionID INT CONSTRAINT FK_PromotionGroupProduct_PromotionID REFERENCES Promotion(PromotionID),
	ProductID INT CONSTRAINT FK_PromotionGroupProduct_ProductID REFERENCES Product(ProductID),
	CONSTRAINT PK_PromotionGroupProduct_ID PRIMARY KEY (PromotionID, ProductID)
)
GO
CREATE TABLE PromotionCategory
(
	PromotionID INT CONSTRAINT FK_PromotionCategory_PromotionID REFERENCES Promotion(PromotionID),
	CategoryID INT CONSTRAINT FK_PromotionCategory_CategoryID REFERENCES Category(CategoryID),
	CONSTRAINT PK_PromotionCategory_PromotionID PRIMARY KEY (PromotionID)
)
GO
CREATE TABLE ExcludeProductPromotionCategory
(
	PromotionID INT CONSTRAINT FK_ExcludeProductPromotionCategory_PromotionID REFERENCES PromotionCategory(PromotionID),
	CategoryID INT CONSTRAINT FK_ExcludeProductPromotionCategory_CategoryID REFERENCES Category(CategoryID),
	ProductID INT CONSTRAINT FK_ExcludeProductPromotionCategory_ProductID REFERENCES Product(ProductID),
	CONSTRAINT PK_ExcludeProductPromotionCategory_ID PRIMARY KEY (PromotionID, CategoryID, ProductID)
)
GO
CREATE TABLE PromotionBrand
(
	PromotionID INT CONSTRAINT FK_PromotionBrand_PromotionID REFERENCES Promotion(PromotionID),
	BrandID INT CONSTRAINT FK_PromotionBrand_BrandID REFERENCES Brand(BrandID),
	CONSTRAINT PK_PromotionBrand_PromotionID PRIMARY KEY (PromotionID)
)
GO
CREATE TABLE ExcludeProductPromotionBrand
(
	PromotionID INT CONSTRAINT FK_ExcludeProductPromotionBrand_PromotionID REFERENCES PromotionBrand(PromotionID),
	BrandID INT CONSTRAINT FK_ExcludeProductPromotionBrand_BrandID REFERENCES Brand(BrandID),
	ProductID INT CONSTRAINT FK_ExcludeProductPromotionBrand_ProductID REFERENCES Product(ProductID),
	CONSTRAINT PK_ExcludeProductPromotionBrand_ID PRIMARY KEY (PromotionID, BrandID, ProductID)
)

/*-------------------- End #Promotion and its relate tables --------------------*/

GO

/*-------------------- Start #Notification and its relate tables --------------------*/

CREATE TABLE [Notification]
(
	NotificationID INT IDENTITY(1,1) PRIMARY KEY,
	NotificationName NVARCHAR(100) NOT NULL,
	NotificationDescription NTEXT,
	ReleaseDate DATETIME
)
GO
CREATE TABLE CustomerNotification
(
	CustomerID NVARCHAR(450) CONSTRAINT FK_CustomerNotification_CustomerID REFERENCES Customer(CustomerID),
	NotificationID INT CONSTRAINT FK_CustomerNotification_NotificationID REFERENCES [Notification](NotificationID),
	NotifyStatus BIT NOT NULL,
	LastUpdate TIMESTAMP NOT NULL,
	CONSTRAINT PK_CustomerNotification_ID PRIMARY KEY(CustomerID, NotificationID)
)
GO
CREATE TABLE StaffNotification
(
	StaffID NVARCHAR(450) CONSTRAINT FK_StaffNotification_StaffID REFERENCES Staff(StaffID),
	NotificationID INT CONSTRAINT FK_StaffNotification_NotificationID REFERENCES [Notification](NotificationID),
	NotifyStatus BIT NOT NULL,
	LastUpdate TIMESTAMP NOT NULL,
	CONSTRAINT PK_StaffNotification_ID PRIMARY KEY(StaffID, NotificationID)
)

/*-------------------- End #Notification and its relate tables --------------------*/

GO

/*-------------------- Start #Review and its relate tables --------------------*/

CREATE TABLE Review
(
	ReviewID INT IDENTITY(1,1) PRIMARY KEY,
	CustomerID NVARCHAR(450) CONSTRAINT FK_Review_CustomerID REFERENCES Customer(CustomerID),
	ProductID INT CONSTRAINT FK_Review_ProductID REFERENCES Product(ProductID),
	Title NVARCHAR(100),
	Content TEXT NOT NULL,
	ReleaseDate DATETIME NOT NULL,
	Rating TINYINT NOT NULL,
	IsBought BIT DEFAULT(0) NOT NULL,
	UsefulCounting INT DEFAULT(0),
	ReplyReviewID INT CONSTRAINT FK_Review_ReviewID REFERENCES Review(ReviewID)
)
GO
CREATE TABLE ReviewConfirm
(
	ReviewID INT CONSTRAINT FK_ReviewConfirm_ReviewID REFERENCES Review(ReviewID),
	IsApproved BIT DEFAULT(1) NOT NULL,
	AssignStaffID NVARCHAR(450) CONSTRAINT FK_ReviewConfirm_AssignStaffID REFERENCES Staff(StaffID),
	StaffComment NVARCHAR(256) DEFAULT(''),
	LastUpdateDate TIMESTAMP NOT NULL,
	CONSTRAINT PK_ReviewConfirm_ReviewID PRIMARY KEY(ReviewID)
)
GO
CREATE TABLE UsefulReview
(
	ReviewID INT CONSTRAINT FK_UsefulReview_ReviewID REFERENCES Review(ReviewID),
	CustomerID NVARCHAR(450) CONSTRAINT FK_UsefulReview_CustomerID REFERENCES Customer(CustomerID),
	CONSTRAINT PK_UsefulReview_ID PRIMARY KEY(ReviewID, CustomerID)
)
GO

/* #Temporary disable review image feature

CREATE TABLE ReviewCommentImage
(
	ReviewID INT CONSTRAINT FK_ReviewCommentImage_ReviewID REFERENCES Review(ReviewID),
	GeneralImageID INT CONSTRAINT FK_ReviewCommentImage_GeneralImageID REFERENCES GeneralImage(GeneralImageID),
	CONSTRAINT PK_ReviewCommentImage_ID PRIMARY KEY(ReviewID, GeneralImageID)
)

*/

/*-------------------- End #Review and its relate tables --------------------*/

GO

/*-------------------- Start #Wishlist and its relate tables --------------------*/

CREATE TABLE WishList
(
	CustomerID NVARCHAR(450) CONSTRAINT FK_WishList_CustomerID REFERENCES Customer(CustomerID),
	ProductID INT CONSTRAINT FK_WishList_ProductID REFERENCES Product(ProductID),
	CONSTRAINT PK_WishList_ID PRIMARY KEY(CustomerID, ProductID)
)

/*-------------------- End #Wishlist and its relate tables --------------------*/

GO

/*-------------------- Start #Cart and its relate tables --------------------*/

CREATE TABLE CustomerCart
(
	CustomerCartID NVARCHAR(450) CONSTRAINT FK_CustomerCart_CustomerID REFERENCES Customer(CustomerID),
	CreateDate DATETIME NOT NULL,
	LastUpdate TIMESTAMP NOT NULL,
	Tax FLOAT DEFAULT(0.15),
	ShippingFee FLOAT DEFAULT(0),
	DisplayPrice FLOAT DEFAULT(0),
	PriceDiscount FLOAT DEFAULT(0),
	TotalPrice FLOAT DEFAULT(0),
	TotalQuantity INT DEFAULT(0),
	PromotionID INT CONSTRAINT FK_CustomerCart_PromotionID REFERENCES Promotion(PromotionID),
	CONSTRAINT PK_CustomerCart_CustomerCartID PRIMARY KEY(CustomerCartID)
)
GO
CREATE TABLE CustomerCartDetail
(
	CustomerCartID NVARCHAR(450) CONSTRAINT FK_CustomerCartDetail_CartID REFERENCES CustomerCart(CustomerCartID),
	ProductID INT CONSTRAINT FK_CustomerCartDetail_ProductID REFERENCES Product(ProductID),
	DisplayPrice FLOAT,
	PriceDiscount FLOAT DEFAULT(0),
	Price FLOAT,
	Quantity INT,
	PromotionID INT CONSTRAINT FK_CustomerCartDetail_PromotionID REFERENCES Promotion(PromotionID),
	CONSTRAINT PK_CustomerCartDetail_ID PRIMARY KEY(CustomerCartID, ProductID)
)
GO
CREATE TABLE AnonymousCustomerCart
(
	AnonymousCustomerCartID NVARCHAR(450) CONSTRAINT FK_AnonymousCustomerCart_CustomerID REFERENCES AnonymousCustomer(AnonymousCustomerID),
	CreateDate DATETIME NOT NULL,
	LastUpdate TIMESTAMP NOT NULL,
	Tax FLOAT DEFAULT(0.15),
	ShippingFee FLOAT DEFAULT(0),
	DisplayPrice FLOAT DEFAULT(0),
	PriceDiscount FLOAT DEFAULT(0),
	TotalPrice FLOAT DEFAULT(0),
	TotalQuantity INT DEFAULT(0),
	PromotionID INT CONSTRAINT FK_AnonymousCustomerCart_PromotionID REFERENCES Promotion(PromotionID),
	CONSTRAINT PK_AnonymousCustomerCart_AnonymousCustomerCartID PRIMARY KEY(AnonymousCustomerCartID)
)
GO
CREATE TABLE AnonymousCustomerCartDetail
(
	AnonymousCustomerCartID NVARCHAR(450) CONSTRAINT FK_AnonymousCustomerCartDetail_CartID REFERENCES AnonymousCustomerCart(AnonymousCustomerCartID),
	ProductID INT CONSTRAINT FK_AnonymousCustomerCartDetail_ProductID REFERENCES Product(ProductID),
	DisplayPrice FLOAT,
	PriceDiscount FLOAT DEFAULT(0),
	Price FLOAT,
	Quantity INT,
	PromotionID INT CONSTRAINT FK_AnonymousCustomerCartDetail_PromotionID REFERENCES Promotion(PromotionID),
	CONSTRAINT PK_AnonymousCustomerCartDetail_ID PRIMARY KEY(AnonymousCustomerCartID, ProductID)
)

/*-------------------- End #Cart and its relate tables --------------------*/

GO

/*-------------------- Start #SaveForLater and its relate tables --------------------*/

CREATE TABLE SaveForLater
(
	CustomerID NVARCHAR(450) CONSTRAINT FK_SaveForLater_CustomerID REFERENCES Customer(CustomerID),
	ProductID INT CONSTRAINT FK_SaveForLater_ProductID REFERENCES Product(ProductID),
	Quantity INT NOT NULL,
	Price FLOAT NOT NULL,
	CONSTRAINT PK_SaveForLater_ID PRIMARY KEY(CustomerID, ProductID)
)

/*-------------------- End #SaveForLater and its relate tables --------------------*/

GO

/*-------------------- Start #Order and its relate tables --------------------*/

CREATE TABLE OrderStatus
(
	OrderStatusID INT IDENTITY(1,1) PRIMARY KEY,
	Code VARCHAR(50) NOT NULL,
	[Name] VARCHAR(100) NOT NULL
)

GO

CREATE TABLE [Order]
(
	OrderID INT IDENTITY(1,1) PRIMARY KEY,
	OrderStatusID INT CONSTRAINT FK_Order_OrderStatusID REFERENCES OrderStatus(OrderStatusID),
	OrderTrackingNumber VARCHAR(20) UNIQUE, /*Auto-generate*/
	CartID NVARCHAR(450),
	OrderDate DATETIME NOT NULL,
	Tax FLOAT DEFAULT(0.15),
	ShippingFee FLOAT DEFAULT(0),
	DisplayPrice FLOAT DEFAULT(0),
	PriceDiscount FLOAT DEFAULT(0),
	TotalPrice FLOAT DEFAULT(0),
	TotalQuantity INT DEFAULT(0),
	PromotionID INT CONSTRAINT FK_Order_PromotionID REFERENCES Promotion(PromotionID),
	FullName NVARCHAR(256),
	AddressLine1 NVARCHAR(255) NOT NULL,
	AddressLine2 NVARCHAR(255),
	City NVARCHAR(100) NOT NULL,
	Zip VARCHAR(50) NOT NULL,
	Phone VARCHAR(20) NOT NULL,
	EstimateShippingDate DATE NOT NULL,
	PaymentToken VARCHAR(250) NOT NULL,
	PaymentStatus VARCHAR(100) NOT NULL,
	InvoiceId VARCHAR(250) NOT NULL
)

GO

CREATE TABLE OrderDetail
(
	OrderID INT CONSTRAINT FK_OrderDetail_OrderID REFERENCES [Order](OrderID),
	ProductID INT CONSTRAINT FK_OrderDetail_ProductID REFERENCES Product(ProductID),
	DisplayPrice FLOAT,
	PriceDiscount FLOAT DEFAULT(0),
	Price FLOAT,
	Quantity INT,
	PromotionID INT CONSTRAINT FK_OrderDetail_PromotionID REFERENCES Promotion(PromotionID),
	CONSTRAINT PK_OrderDetail_ID PRIMARY KEY(OrderID, ProductID)
)

GO


/*-------------------- End #Order and its relate tables --------------------*/