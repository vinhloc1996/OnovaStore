using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Data
{
    public class OnovaDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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

        public OnovaDbContext(DbContextOptions<OnovaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AnonymousCustomer>(entity =>
            {
                entity.Property(e => e.AnonymousCustomerId)
                    .HasColumnName("AnonymousCustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Ipaddress)
                    .IsRequired()
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdateDate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.VisitDate).HasColumnType("datetime");
            });

            builder.Entity<AnonymousCustomerCart>(entity =>
            {
                entity.HasKey(e => e.AnonymousCustomerId);

                entity.HasIndex(e => e.AnonymousCustomerCartId)
                    .HasName("UQ__Anonymou__71DF0F7EC0C6910C")
                    .IsUnique();

                entity.Property(e => e.AnonymousCustomerId)
                    .HasColumnName("AnonymousCustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnonymousCustomerCartId).HasColumnName("AnonymousCustomerCartID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastUpdate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.Tax).HasDefaultValueSql("((0.15))");

                entity.Property(e => e.TotalPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantity).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.AnonymousCustomer)
                    .WithOne(p => p.AnonymousCustomerCart)
                    .HasForeignKey<AnonymousCustomerCart>(d => d.AnonymousCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnonymousCustomerCart_CustomerID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.AnonymousCustomerCart)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_AnonymousCustomerCart_PromotionID");
            });

            builder.Entity<AnonymousCustomerCartDetail>(entity =>
            {
                entity.HasKey(e => new { e.AnonymousCustomerCartId, e.ProductId });

                entity.Property(e => e.AnonymousCustomerCartId).HasColumnName("AnonymousCustomerCartID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.HasOne(d => d.AnonymousCustomerCart)
                    .WithMany(p => p.AnonymousCustomerCartDetail)
                    .HasPrincipalKey(p => p.AnonymousCustomerCartId)
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

            builder.Entity<Brand>(entity =>
            {
                entity.HasIndex(e => e.ContactEmail)
                    .HasName("UQ__Brand__FFA796CD6F298EB1")
                    .IsUnique();

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AddressLine2).HasMaxLength(255);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactEmail)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName).HasMaxLength(255);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContactTitle)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BrandImageNavigation)
                    .WithMany(p => p.Brand)
                    .HasForeignKey(d => d.BrandImage)
                    .HasConstraintName("FK_Brand_GeneralImageID");
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.CategoryCode)
                    .HasName("UQ__Category__371BA9550BB1A02D")
                    .IsUnique();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryCode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

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

            builder.Entity<UserStatus>(entity =>
            {
                entity.Property(e => e.UserStatusId).HasColumnName("UserStatusID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            builder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.StaffId)
                    .HasColumnName("StaffID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddBy).HasMaxLength(450);

                entity.Property(e => e.AddDate).HasColumnType("datetime");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserStatusId).HasColumnName("UserStatusID");

                entity.HasOne(d => d.AddByManagerStaff)
                    .WithMany(p => p.InverseAddByNavigation)
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

            builder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnonymouseCustomerId)
                    .HasColumnName("AnonymouseCustomerID")
                    .HasMaxLength(450);

                entity.Property(e => e.JoinDate).HasColumnType("datetime");

                entity.Property(e => e.UserStatusId).HasColumnName("UserStatusID");

                entity.HasOne(d => d.AnonymouseCustomer)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.AnonymouseCustomerId)
                    .HasConstraintName("FK_AnonymousCustomer_AnonymousCustomerID");

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

            builder.Entity<CustomerCart>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.HasIndex(e => e.CustomerCartId)
                    .HasName("UQ__Customer__EE78A1EDD2C7578E")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerCartId).HasColumnName("CustomerCartID");

                entity.Property(e => e.DisplayPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastUpdate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.Tax).HasDefaultValueSql("((0.15))");

                entity.Property(e => e.TotalPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantity).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithOne(p => p.CustomerCart)
                    .HasForeignKey<CustomerCart>(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCart_CustomerID");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.CustomerCart)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_CustomerCart_PromotionID");
            });

            builder.Entity<CustomerCartDetail>(entity =>
            {
                entity.HasKey(e => new { e.CustomerCartId, e.ProductId });

                entity.Property(e => e.CustomerCartId).HasColumnName("CustomerCartID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.PriceDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.HasOne(d => d.CustomerCart)
                    .WithMany(p => p.CustomerCartDetail)
                    .HasPrincipalKey(p => p.CustomerCartId)
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

            builder.Entity<CustomerNotification>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.NotificationId });

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.LastUpdate)
                    .IsRequired()
                    .IsRowVersion();

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

            builder.Entity<CustomerPurchaseInfo>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TotalMoneySpend).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalQuantityOfPurchasedProduct).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalSuccessOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithOne(p => p.CustomerPurchaseInfo)
                    .HasForeignKey<CustomerPurchaseInfo>(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPurchaseInfo_CustomerID");
            });

            builder.Entity<CustomerRecentView>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

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

            builder.Entity<ExcludeProductPromotionBrand>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.BrandId, e.ProductId });

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

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

            builder.Entity<ExcludeProductPromotionCategory>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.CategoryId, e.ProductId });

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

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

            builder.Entity<GeneralImage>(entity =>
            {
                entity.Property(e => e.GeneralImageId).HasColumnName("GeneralImageID");

                entity.Property(e => e.AddDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasColumnName("ImageURL")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StaffId)
                    .HasColumnName("StaffID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.GeneralImage)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_GeneralImage_StaffID");
            });

            builder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.NotificationDescription).HasColumnType("ntext");

                entity.Property(e => e.NotificationName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            });

            builder.Entity<Option>(entity =>
            {
                entity.Property(e => e.OptionId).HasColumnName("OptionID");

                entity.Property(e => e.OptionName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.Entity<OptionDetail>(entity =>
            {
                entity.Property(e => e.OptionDetailId).HasColumnName("OptionDetailID");

                entity.Property(e => e.OptionDetailName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OptionId).HasColumnName("OptionID");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.OptionDetail)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK_OptionDetail_OptionID");
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.ProductCode)
                    .HasName("UQ__Product__2F4E024F3574CE52")
                    .IsUnique();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.AddDate).HasColumnType("datetime");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.LastUpdateDate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ParentProductId).HasColumnName("ParentProductID");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProductLongDesc)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.ProductShortDesc)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.Rating).HasDefaultValueSql("((0))");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);

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

            builder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.GeneralImageId });

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.GeneralImageId).HasColumnName("GeneralImageID");

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

            builder.Entity<ProductNotification>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.Email });

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Email)
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductNotification)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductNotification_ProductID");
            });

            builder.Entity<ProductOptionGroup>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OptionId, e.OptionDetailId });

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.OptionId).HasColumnName("OptionID");

                entity.Property(e => e.OptionDetailId).HasColumnName("OptionDetailID");

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

            builder.Entity<ProductPriceOff>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.PercentOff).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.ProductPriceOff)
                    .HasForeignKey<ProductPriceOff>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPriceOff_ProductID");
            });

            builder.Entity<ProductSpecification>(entity =>
            {
                entity.Property(e => e.ProductSpecificationId).HasColumnName("ProductSpecificationID");

                entity.Property(e => e.ProductSpecificationName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.Entity<ProductSprcificationValue>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProductSpecificationId });

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductSpecificationId).HasColumnName("ProductSpecificationID");

                entity.Property(e => e.ProductSpecificationValue)
                    .IsRequired()
                    .HasMaxLength(255);

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

            builder.Entity<ProductStatus>(entity =>
            {
                entity.HasIndex(e => e.StatusCode)
                    .HasName("UQ__ProductS__6A7B44FC2CACFE6F")
                    .IsUnique();

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StatusName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            builder.Entity<Promotion>(entity =>
            {
                entity.HasIndex(e => e.PromotionCode)
                    .HasName("UQ__Promotio__A617E4B6B4607E71")
                    .IsUnique();

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateDate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.PercentOff).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PromotionCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PromotionDescription).HasColumnType("ntext");

                entity.Property(e => e.PromotionName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PromotionStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TargetApply)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PromotionImageNavigation)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.PromotionImage)
                    .HasConstraintName("FK_Promotion_GeneralImageID");
            });

            builder.Entity<PromotionBrand>(entity =>
            {
                entity.HasKey(e => e.PromotionId);

                entity.Property(e => e.PromotionId)
                    .HasColumnName("PromotionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

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

            builder.Entity<PromotionCategory>(entity =>
            {
                entity.HasKey(e => e.PromotionId);

                entity.Property(e => e.PromotionId)
                    .HasColumnName("PromotionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

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

            builder.Entity<PromotionGroupProduct>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.ProductId });

                entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

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

            builder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasMaxLength(450);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

                entity.Property(e => e.ReplyReviewId).HasColumnName("ReplyReviewID");

                entity.Property(e => e.Title).HasMaxLength(100);

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

            builder.Entity<ReviewConfirm>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.Property(e => e.ReviewId)
                    .HasColumnName("ReviewID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignStaffId)
                    .HasColumnName("AssignStaffID")
                    .HasMaxLength(450);

                entity.Property(e => e.IsApproved).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdateDate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.StaffComment)
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('')");

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

            builder.Entity<SaveForLater>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

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

            builder.Entity<ShippingInfo>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Shipping__A9D10534837E440F")
                    .IsUnique();

                entity.Property(e => e.ShippingInfoId).HasColumnName("ShippingInfoID");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AddressLine2).HasMaxLength(255);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasMaxLength(450);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(256);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ShippingInfo)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ShippingInfo_CustomerID");
            });

            builder.Entity<StaffNotification>(entity =>
            {
                entity.HasKey(e => new { e.StaffId, e.NotificationId });

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.LastUpdate)
                    .IsRequired()
                    .IsRowVersion();

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

            builder.Entity<Story>(entity =>
            {
                entity.Property(e => e.StoryId).HasColumnName("StoryID");

                entity.Property(e => e.LastUpdateDate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.StoryDescription)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.StoryTitle)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            builder.Entity<Subscriber>(entity =>
            {
                entity.HasKey(e => e.SubscribeEmail);

                entity.Property(e => e.SubscribeEmail)
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastUpdateDate)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SubscribedDate).HasColumnType("datetime");

                entity.Property(e => e.UnsubscribeToken)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnsubscribeTokenExpire).HasColumnType("datetime");
            });

            builder.Entity<SubscribeStory>(entity =>
            {
                entity.HasKey(e => new { e.SubscribeEmail, e.StoryId });

                entity.Property(e => e.SubscribeEmail)
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.StoryId).HasColumnName("StoryID");

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

            builder.Entity<UsefulReview>(entity =>
            {
                entity.HasKey(e => new { e.ReviewId, e.CustomerId });

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

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

            builder.Entity<UserStatus>(entity =>
            {
                entity.Property(e => e.UserStatusId).HasColumnName("UserStatusID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            builder.Entity<WishList>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

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

            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            builder.Entity<ApplicationUser>().Property(p => p.Id).HasColumnName("UserID");
            builder.Entity<IdentityRole>().Property(p => p.Id).HasColumnName("RoleID");
            builder.Entity<IdentityRoleClaim<string>>().Property(p => p.Id).HasColumnName("RoleClaimID");
            builder.Entity<IdentityUserClaim<string>>().Property(p => p.Id).HasColumnName("UserClaimID");
        }
    }
}