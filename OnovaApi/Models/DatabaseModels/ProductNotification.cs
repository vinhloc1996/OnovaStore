using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductNotification
    {
        [Column("ProductID")]
        public int ProductId { get; set; }
        [StringLength(254)]
        public string Email { get; set; }
        public bool NotifyStatus { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductNotification")]
        public Product Product { get; set; }
    }
}
