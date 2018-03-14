using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ShippingInfo
    {
        [Column("ShippingInfoID")]
        public int ShippingInfoId { get; set; }
        [StringLength(256)]
        public string FullName { get; set; }
        [Required]
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [StringLength(255)]
        public string AddressLine2 { get; set; }
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        public string Zip { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [Column("CustomerID")]
        [StringLength(450)]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("ShippingInfo")]
        public Customer Customer { get; set; }
    }
}
