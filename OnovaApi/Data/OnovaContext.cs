﻿using Microsoft.AspNetCore.Identity;
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
        public virtual DbSet<GeneralImage> GeneralImage { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductNotification> ProductNotification { get; set; }
        public virtual DbSet<ProductStatus> ProductStatus { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionBrand> PromotionBrand { get; set; }
        public virtual DbSet<PromotionCategory> PromotionCategory { get; set; }
        public virtual DbSet<ShippingInfo> ShippingInfo { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }

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
                entity.Property(e => e.ContactEmail).IsUnicode(false);

                entity.Property(e => e.TotalProduct).HasDefaultValueSql("((0))");

                entity.Property(e => e.ContactPhone).IsUnicode(false);

                entity.Property(e => e.ContactTitle).IsUnicode(false);

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.HasOne(d => d.BrandImageNavigation)
                    .WithMany(p => p.Brand)
                    .HasForeignKey(d => d.BrandImage)
                    .HasConstraintName("FK_Brand_GeneralImageID");
            });

            modelBuilder.Entity<Category>(entity =>
            {
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

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_CustomerID");
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

            modelBuilder.Entity<GeneralImage>(entity =>
            {
                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.GeneralImage)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_GeneralImage_StaffID");
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

                entity.Property(e => e.PercentOff).HasColumnType("decimal(18,2)");

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
