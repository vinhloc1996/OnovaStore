using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Customer = new HashSet<Customer>();
            Staff = new HashSet<Staff>();
        }

        public int UserStatusId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<Customer> Customer { get; set; }
        public ICollection<Staff> Staff { get; set; }
    }
}
