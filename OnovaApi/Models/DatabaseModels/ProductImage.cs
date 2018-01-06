using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductImage
    {
        public int ProductId { get; set; }
        public int GeneralImageId { get; set; }

        public GeneralImage GeneralImage { get; set; }
        public Product Product { get; set; }
    }
}
