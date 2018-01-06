using System;
using System.Collections.Generic;

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
            ProductImage = new HashSet<ProductImage>();
            ProductNotification = new HashSet<ProductNotification>();
            ProductOptionGroup = new HashSet<ProductOptionGroup>();
            ProductSprcificationValue = new HashSet<ProductSprcificationValue>();
            PromotionGroupProduct = new HashSet<PromotionGroupProduct>();
            Review = new HashSet<Review>();
            SaveForLater = new HashSet<SaveForLater>();
            WishList = new HashSet<WishList>();
        }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public double? Weight { get; set; }
        public double RealPrice { get; set; }
        public double DisplayPrice { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductLongDesc { get; set; }
        public int? ProductThumbImage { get; set; }
        public string Slug { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int TotalQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public int MaximumQuantity { get; set; }
        public DateTime AddDate { get; set; }
        public byte[] LastUpdateDate { get; set; }
        public float? Rating { get; set; }
        public int? WishCounting { get; set; }
        public int? ProductStatusId { get; set; }
        public int? ParentProductId { get; set; }

        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public Product ParentProduct { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public GeneralImage ProductThumbImageNavigation { get; set; }
        public ProductPriceOff ProductPriceOff { get; set; }
        public ICollection<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
        public ICollection<CustomerCartDetail> CustomerCartDetail { get; set; }
        public ICollection<CustomerRecentView> CustomerRecentView { get; set; }
        public ICollection<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
        public ICollection<ExcludeProductPromotionCategory> ExcludeProductPromotionCategory { get; set; }
        public ICollection<Product> InverseParentProduct { get; set; }
        public ICollection<ProductImage> ProductImage { get; set; }
        public ICollection<ProductNotification> ProductNotification { get; set; }
        public ICollection<ProductOptionGroup> ProductOptionGroup { get; set; }
        public ICollection<ProductSprcificationValue> ProductSprcificationValue { get; set; }
        public ICollection<PromotionGroupProduct> PromotionGroupProduct { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<SaveForLater> SaveForLater { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
