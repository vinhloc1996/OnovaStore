using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Data
{
    public partial class OnovaContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public virtual DbSet<AnonymousCustomer> AnonymousCustomer { get; set; }
        public virtual DbSet<AnonymousCustomerCart> AnonymousCustomerCart { get; set; }
        public virtual DbSet<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerCart> CustomerCart { get; set; }
        public virtual DbSet<CustomerCartDetail> CustomerCartDetail { get; set; }
        public virtual DbSet<CustomerNotification> CustomerNotification { get; set; }
        public virtual DbSet<CustomerPurchaseInfo> CustomerPurchaseInfo { get; set; }
        public virtual DbSet<CustomerRecentView> CustomerRecentView { get; set; }
        public virtual DbSet<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
        public virtual DbSet<ExcludeProductPromotionCategory> ExcludeProductPromotionCategory { get; set; }
        public virtual DbSet<GeneralImage> GeneralImage { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Option> Option { get; set; }
        public virtual DbSet<OptionDetail> OptionDetail { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductNotification> ProductNotification { get; set; }
        public virtual DbSet<ProductOptionGroup> ProductOptionGroup { get; set; }
        public virtual DbSet<ProductPriceOff> ProductPriceOff { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecification { get; set; }
        public virtual DbSet<ProductSprcificationValue> ProductSprcificationValue { get; set; }
        public virtual DbSet<ProductStatus> ProductStatus { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionBrand> PromotionBrand { get; set; }
        public virtual DbSet<PromotionCategory> PromotionCategory { get; set; }
        public virtual DbSet<PromotionGroupProduct> PromotionGroupProduct { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<ReviewConfirm> ReviewConfirm { get; set; }
        public virtual DbSet<SaveForLater> SaveForLater { get; set; }
        public virtual DbSet<ShippingInfo> ShippingInfo { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StaffNotification> StaffNotification { get; set; }
        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<Subscriber> Subscriber { get; set; }
        public virtual DbSet<SubscribeStory> SubscribeStory { get; set; }
        public virtual DbSet<UsefulReview> UsefulReview { get; set; }
        public virtual DbSet<UserStatus> UserStatus { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }

        public OnovaContext(DbContextOptions<OnovaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AnonymousCustomer>(entity =>
            {
                entity.Property(e => e.AnonymousCustomerId).ValueGeneratedNever();

                entity.Property(e => e.Ipaddress).IsUnicode(false);

                entity.Property(e => e.LastUpdateDate).IsRowVersion();
            });

            modelBuilder.Entity<AnonymousCustomerCart>(entity =>
            {
                entity.Property(e => e.AnonymousCustomerCartId).ValueGeneratedNever();

                entity.Property(e => e.DisplayPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastUpdate).IsRowVersion();

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShippingFee).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tax).HasDefaultValueSql("((0.15))");

                entity.Property(e => e.TotalPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantity).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.AnonymousCustomerCartNavigation)
                    .WithOne(p => p.AnonymousCustomerCart)
                    .HasForeignKey<AnonymousCustomerCart>(d => d.AnonymousCustomerCartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnonymousCustomerCart_AnonymousCustomerCartID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.AnonymousCustomerCart)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_AnonymousCustomerCart_PromotionID");
            });

            modelBuilder.Entity<AnonymousCustomerCartDetail>(entity =>
            {
                entity.HasKey(e => new { e.AnonymousCustomerCartId, e.ProductId });

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.AnonymousCustomerCart)
                    .WithMany(p => p.AnonymousCustomerCartDetail)
                    .HasForeignKey(d => d.AnonymousCustomerCartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnonymousCustomerCartDetail_CartID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.AnonymousCustomerCartDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnonymousCustomerCartDetail_ProductID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.AnonymousCustomerCartDetail)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_AnonymousCustomerCartDetail_PromotionID");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasIndex(e => e.ContactEmail)
                    .HasName("UQ__Brand__FFA796CD1509DF89")
                    .IsUnique();

                entity.Property(e => e.ContactEmail).IsUnicode(false);

                entity.Property(e => e.ContactPhone).IsUnicode(false);

                entity.Property(e => e.ContactTitle).IsUnicode(false);

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.BrandImageNavigation)
                    .WithMany(p => p.Brand)
                    .HasForeignKey(d => d.BrandImage)
                    .HasConstraintName("FK_Brand_GeneralImageID");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.CategoryCode)
                    .HasName("UQ__Category__371BA955A542D42E")
                    .IsUnique();

                entity.Property(e => e.CategoryCode).IsUnicode(false);

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.Property(e => e.TotalProduct).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CategoryImageNavigation)
                    .WithMany(p => p.Category)
                    .HasForeignKey(d => d.CategoryImage)
                    .HasConstraintName("FK_Category_GeneralImageID");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_Category_CategoryID");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.HasOne(d => d.AnonymouseCustomer)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.AnonymouseCustomerId)
                    .HasConstraintName("FK_Customer_AnonymousCustomerID");

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_CustomerID");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.UserStatusId)
                    .HasConstraintName("FK_Customer_UserStatusID");
            });

            modelBuilder.Entity<CustomerCart>(entity =>
            {
                entity.Property(e => e.CustomerCartId).ValueGeneratedNever();

                entity.Property(e => e.DisplayPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastUpdate).IsRowVersion();

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShippingFee).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tax).HasDefaultValueSql("((0.15))");

                entity.Property(e => e.TotalPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantity).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CustomerCartNavigation)
                    .WithOne(p => p.CustomerCart)
                    .HasForeignKey<CustomerCart>(d => d.CustomerCartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCart_CustomerID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.CustomerCart)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_CustomerCart_PromotionID");
            });

            modelBuilder.Entity<CustomerCartDetail>(entity =>
            {
                entity.HasKey(e => new { e.CustomerCartId, e.ProductId });

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CustomerCart)
                    .WithMany(p => p.CustomerCartDetail)
                    .HasForeignKey(d => d.CustomerCartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCartDetail_CartID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerCartDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCartDetail_ProductID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.CustomerCartDetail)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_CustomerCartDetail_PromotionID");
            });

            modelBuilder.Entity<CustomerNotification>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.NotificationId });

                entity.Property(e => e.LastUpdate).IsRowVersion();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerNotification)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerNotification_CustomerID");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.CustomerNotification)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerNotification_NotificationID");
            });

            modelBuilder.Entity<CustomerPurchaseInfo>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.TotalMoneySpend).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantityOfPurchasedProduct).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalSuccessOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithOne(p => p.CustomerPurchaseInfo)
                    .HasForeignKey<CustomerPurchaseInfo>(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPurchaseInfo_CustomerID");
            });

            modelBuilder.Entity<CustomerRecentView>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerRecentView)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerRecentView_CustomerID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerRecentView)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerRecentView_ProductID");
            });

            modelBuilder.Entity<ExcludeProductPromotionBrand>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.BrandId, e.ProductId });

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ExcludeProductPromotionBrand)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcludeProductPromotionBrand_BrandID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ExcludeProductPromotionBrand)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcludeProductPromotionBrand_ProductID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.ExcludeProductPromotionBrand)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcludeProductPromotionBrand_PromotionID");
            });

            modelBuilder.Entity<ExcludeProductPromotionCategory>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.CategoryId, e.ProductId });

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ExcludeProductPromotionCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcludeProductPromotionCategory_CategoryID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ExcludeProductPromotionCategory)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcludeProductPromotionCategory_ProductID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.ExcludeProductPromotionCategory)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExcludeProductPromotionCategory_PromotionID");
            });

            modelBuilder.Entity<GeneralImage>(entity =>
            {
                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.GeneralImage)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_GeneralImage_StaffID");
            });

            modelBuilder.Entity<OptionDetail>(entity =>
            {
                entity.HasOne(d => d.Option)
                    .WithMany(p => p.OptionDetail)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK_OptionDetail_OptionID");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.OrderTrackingNumber)
                    .HasName("UQ__Order__02983D835064F442")
                    .IsUnique();

                entity.Property(e => e.DisplayPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.InvoiceTokenId).IsUnicode(false);

                entity.Property(e => e.OrderTrackingNumber).IsUnicode(false);

                entity.Property(e => e.PaymentStatus).IsUnicode(false);

                entity.Property(e => e.PaymentTokenId).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShippingFee).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tax).HasDefaultValueSql("((0.15))");

                entity.Property(e => e.TotalPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_Order_OrderStatusID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_Order_PromotionID");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_OrderID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_ProductID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_OrderDetail_PromotionID");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.ProductCode)
                    .HasName("UQ__Product__2F4E024F48BD80E6")
                    .IsUnique();

                entity.Property(e => e.LastUpdateDate).IsRowVersion();

                entity.Property(e => e.ProductCode).IsUnicode(false);

                entity.Property(e => e.Rating).HasDefaultValueSql("((0))");

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.Property(e => e.WishCounting).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Product_BrandID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_CategoryID");

                entity.HasOne(d => d.ParentProduct)
                    .WithMany(p => p.InverseParentProduct)
                    .HasForeignKey(d => d.ParentProductId)
                    .HasConstraintName("FK_Product_ProductID");

                entity.HasOne(d => d.ProductStatus)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductStatusId)
                    .HasConstraintName("FK_Product_ProductStatusID");

                entity.HasOne(d => d.ProductThumbImageNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductThumbImage)
                    .HasConstraintName("FK_Product_ProductThumbImage");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.GeneralImageId });

                entity.HasOne(d => d.GeneralImage)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(d => d.GeneralImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImage_GeneralImageID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImage_ProductID");
            });

            modelBuilder.Entity<ProductNotification>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.Email });

                entity.Property(e => e.Email).IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductNotification)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductNotification_ProductID");
            });

            modelBuilder.Entity<ProductOptionGroup>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OptionId, e.OptionDetailId });

                entity.HasOne(d => d.OptionDetail)
                    .WithMany(p => p.ProductOptionGroup)
                    .HasForeignKey(d => d.OptionDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOptionGroup_OptionDetailID");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.ProductOptionGroup)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOptionGroup_OptionID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOptionGroup)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOptionGroup_ProductID");
            });

            modelBuilder.Entity<ProductPriceOff>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.ProductPriceOff)
                    .HasForeignKey<ProductPriceOff>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPriceOff_ProductID");
            });

            modelBuilder.Entity<ProductSprcificationValue>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProductSpecificationId });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSprcificationValue)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSprcificationValue_ProductID");

                entity.HasOne(d => d.ProductSpecification)
                    .WithMany(p => p.ProductSprcificationValue)
                    .HasForeignKey(d => d.ProductSpecificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSprcificationValue_ProductSpecificationID");
            });

            modelBuilder.Entity<ProductStatus>(entity =>
            {
                entity.HasIndex(e => e.StatusCode)
                    .HasName("UQ__ProductS__6A7B44FC27401866")
                    .IsUnique();

                entity.Property(e => e.StatusCode).IsUnicode(false);

                entity.Property(e => e.StatusDescription).IsUnicode(false);

                entity.Property(e => e.StatusName).IsUnicode(false);
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasIndex(e => e.PromotionCode)
                    .HasName("UQ__Promotio__A617E4B6943E9D2E")
                    .IsUnique();

                entity.Property(e => e.LastUpdateDate).IsRowVersion();

                entity.Property(e => e.PromotionCode).IsUnicode(false);

                entity.HasOne(d => d.PromotionImageNavigation)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.PromotionImage)
                    .HasConstraintName("FK_Promotion_GeneralImageID");
            });

            modelBuilder.Entity<PromotionBrand>(entity =>
            {
                entity.Property(e => e.PromotionId).ValueGeneratedNever();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.PromotionBrand)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_PromotionBrand_BrandID");

                entity.HasOne(d => d.Promotion)
                    .WithOne(p => p.PromotionBrand)
                    .HasForeignKey<PromotionBrand>(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionBrand_PromotionID");
            });

            modelBuilder.Entity<PromotionCategory>(entity =>
            {
                entity.Property(e => e.PromotionId).ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.PromotionCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_PromotionCategory_CategoryID");

                entity.HasOne(d => d.Promotion)
                    .WithOne(p => p.PromotionCategory)
                    .HasForeignKey<PromotionCategory>(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionCategory_PromotionID");
            });

            modelBuilder.Entity<PromotionGroupProduct>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.ProductId });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PromotionGroupProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionGroupProduct_ProductID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionGroupProduct)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionGroupProduct_PromotionID");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.UsefulCounting).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Review_CustomerID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Review_ProductID");

                entity.HasOne(d => d.ReplyReview)
                    .WithMany(p => p.InverseReplyReview)
                    .HasForeignKey(d => d.ReplyReviewId)
                    .HasConstraintName("FK_Review_ReviewID");
            });

            modelBuilder.Entity<ReviewConfirm>(entity =>
            {
                entity.Property(e => e.ReviewId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdateDate).IsRowVersion();

                entity.Property(e => e.StaffComment).HasDefaultValueSql("('')");

                entity.HasOne(d => d.AssignStaff)
                    .WithMany(p => p.ReviewConfirm)
                    .HasForeignKey(d => d.AssignStaffId)
                    .HasConstraintName("FK_ReviewConfirm_AssignStaffID");

                entity.HasOne(d => d.Review)
                    .WithOne(p => p.ReviewConfirm)
                    .HasForeignKey<ReviewConfirm>(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewConfirm_ReviewID");
            });

            modelBuilder.Entity<SaveForLater>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SaveForLater)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SaveForLater_CustomerID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SaveForLater)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SaveForLater_ProductID");
            });

            modelBuilder.Entity<ShippingInfo>(entity =>
            {
                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ShippingInfo)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ShippingInfo_CustomerID");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.StaffId).ValueGeneratedNever();

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.AddByStaffManager)
                    .WithMany(p => p.InverseAddByStaffManager)
                    .HasForeignKey(d => d.AddBy)
                    .HasConstraintName("FK_Staff_StaffManagerID");

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne(p => p.Staff)
                    .HasForeignKey<Staff>(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_StaffID");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.UserStatusId)
                    .HasConstraintName("FK_Staff_UserStatusID");
            });

            modelBuilder.Entity<StaffNotification>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.NotificationId });

                entity.Property(e => e.LastUpdate).IsRowVersion();

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.StaffNotification)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StaffNotification_NotificationID");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffNotification)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StaffNotification_StaffID");
            });

            modelBuilder.Entity<Story>(entity =>
            {
                entity.Property(e => e.LastUpdateDate).IsRowVersion();
            });

            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.Property(e => e.SubscribeEmail)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastUpdateDate).IsRowVersion();

                entity.Property(e => e.UnsubscribeToken)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SubscribeStory>(entity =>
            {
                entity.HasKey(e => new { e.SubscribeEmail, e.StoryId });

                entity.Property(e => e.SubscribeEmail).IsUnicode(false);

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.SubscribeStory)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubscribeStory_StoryID");

                entity.HasOne(d => d.SubscribeEmailNavigation)
                    .WithMany(p => p.SubscribeStory)
                    .HasForeignKey(d => d.SubscribeEmail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubscribeStory_SubcriberEmail");
            });

            modelBuilder.Entity<UsefulReview>(entity =>
            {
                entity.HasKey(e => new { e.ReviewId, e.CustomerId });

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.UsefulReview)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsefulReview_CustomerID");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.UsefulReview)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsefulReview_ReviewID");
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WishList_CustomerID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WishList_ProductID");
            });

            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Id).HasColumnName("UserID");
            modelBuilder.Entity<ApplicationRole>().Property(p => p.Id).HasColumnName("RoleID");
            modelBuilder.Entity<IdentityRoleClaim<string>>().Property(p => p.Id).HasColumnName("RoleClaimID");
            modelBuilder.Entity<IdentityUserClaim<string>>().Property(p => p.Id).HasColumnName("UserClaimID");
        }
    }
}
