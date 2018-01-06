using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductNotification
    {
        public int ProductId { get; set; }
        public string Email { get; set; }
        public bool NotifyStatus { get; set; }

        public Product Product { get; set; }
    }
}
