using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class AnonymousCustomer
    {
        public AnonymousCustomer()
        {
            Customer = new HashSet<Customer>();
        }

        public string AnonymousCustomerId { get; set; }
        public DateTime VisitDate { get; set; }
        public byte[] LastUpdateDate { get; set; }
        public string Ipaddress { get; set; }

        public AnonymousCustomerCart AnonymousCustomerCart { get; set; }
        public ICollection<Customer> Customer { get; set; }
    }
}
