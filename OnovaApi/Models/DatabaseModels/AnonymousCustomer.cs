using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class AnonymousCustomer
    {
        public AnonymousCustomer()
        {
            Customer = new HashSet<Customer>();
        }

        [Column("AnonymousCustomerID")]
        public string AnonymousCustomerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime VisitDate { get; set; }
        [Required]
        public byte[] LastUpdateDate { get; set; }
        [Required]
        [Column("IPAddress")]
        [StringLength(50)]
        public string Ipaddress { get; set; }

        [InverseProperty("AnonymousCustomerCartNavigation")]
        public AnonymousCustomerCart AnonymousCustomerCart { get; set; }
        [InverseProperty("AnonymouseCustomer")]
        public ICollection<Customer> Customer { get; set; }
    }
}
