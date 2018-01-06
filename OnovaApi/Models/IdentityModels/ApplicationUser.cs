using System;
using Microsoft.AspNetCore.Identity;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FullName { get; set; }
        public string Picture { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public Customer Customer { get; set; }
        public Staff Staff { get; set; }
    }
}
