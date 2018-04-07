using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Product
    {
        public Product()
        {
            AnonymousCustomerCartDetail = new HashSet<AnonymousCustomerCartDetail>();
            CustomerCartDetail = new HashSet<CustomerCartDetail>();
            CustomerRecentView = new HashSet<CustomerRecentView>();
            ExcludeProductPromotionBrand = new HashSet<ExcludeProductPromotionBrand>();
            ExcludeProductPromotionCategory = new HashSet<ExcludeProductPromotionCategory>();
            InverseParentProduct = new HashSet<Product>();
            OrderDetail = new HashSet<OrderDetail>();
            ProductImage = new HashSet<ProductImage>();
            ProductNotification = new HashSet<ProductNotification>();
            ProductOptionGroup = new HashSet<ProductOptionGroup>();
            ProductSprcificationValue = new HashSet<ProductSprcificationValue>();
            PromotionGroupProduct = new HashSet<PromotionGroupProduct>();
            Review = new HashSet<Review>();
            SaveForLater = new HashSet<SaveForLater>();
            WishList = new HashSet<WishList>();
        }

        [Column("ProductID")]
        public int ProductId { get; set; }
        public bool? IsHide { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public double? Weight { get; set; }
        public double RealPrice { get; set; }
        public double DisplayPrice { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductShortDesc { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string ProductLongDesc { get; set; }
        public string ProductThumbImage { get; set; }
        [Required]
        [StringLength(400)]
        public string Slug { get; set; }
        [Column("CategoryID")]
        public int? CategoryId { get; set; }
        [Column("BrandID")]
        public int? BrandId { get; set; }
        public int CurrentQuantity { get; set; }
        public int MaximumQuantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddDate { get; set; }
        public float? Rating { get; set; }
        public int? WishCounting { get; set; }
        [Column("ProductStatusID")]
        public int? ProductStatusId { get; set; }
        [Column("ParentProductID")]
        public int? ParentProductId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("Product")]
        public Brand Brand { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Product")]
        public Category Category { get; set; }
        [ForeignKey("ParentProductId")]
        [InverseProperty("InverseParentProduct")]
        public Product ParentProduct { get; set; }
        [ForeignKey("ProductStatusId")]
        [InverseProperty("Product")]
        public ProductStatus ProductStatus { get; set; }
        [ForeignKey("ProductThumbImage")]
        [InverseProperty("Product")]
        public GeneralImage ProductThumbImageNavigation { get; set; }
        [InverseProperty("Product")]
        public ProductPriceOff ProductPriceOff { get; set; }
        [InverseProperty("Product")]
        public ICollection<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
        [InverseProperty("Product")]
        public ICollection<CustomerCartDetail> CustomerCartDetail { get; set; }
        [InverseProperty("Product")]
        public ICollection<CustomerRecentView> CustomerRecentView { get; set; }
        [InverseProperty("Product")]
        public ICollection<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
        [InverseProperty("Product")]
        public ICollection<ExcludeProductPromotionCategory> ExcludeProductPromotionCategory { get; set; }
        [InverseProperty("ParentProduct")]
        public ICollection<Product> InverseParentProduct { get; set; }
        [InverseProperty("Product")]
        public ICollection<OrderDetail> OrderDetail { get; set; }
        [InverseProperty("Product")]
        public ICollection<ProductImage> ProductImage { get; set; }
        [InverseProperty("Product")]
        public ICollection<ProductNotification> ProductNotification { get; set; }
        [InverseProperty("Product")]
        public ICollection<ProductOptionGroup> ProductOptionGroup { get; set; }
        [InverseProperty("Product")]
        public ICollection<ProductSprcificationValue> ProductSprcificationValue { get; set; }
        [InverseProperty("Product")]
        public ICollection<PromotionGroupProduct> PromotionGroupProduct { get; set; }
        [InverseProperty("Product")]
        public ICollection<Review> Review { get; set; }
        [InverseProperty("Product")]
        public ICollection<SaveForLater> SaveForLater { get; set; }
        [InverseProperty("Product")]
        public ICollection<WishList> WishList { get; set; }
    }
}
