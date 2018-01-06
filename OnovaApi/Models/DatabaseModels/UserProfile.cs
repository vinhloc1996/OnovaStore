using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class UserProfile : IdentityUser
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public IdentityUser User { get; set; }
        public Customer Customer { get; set; }
        public Staff Staff { get; set; }
    }
}
