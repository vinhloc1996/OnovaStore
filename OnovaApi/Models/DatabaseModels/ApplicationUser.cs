using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnovaApi.Models.DatabaseModels
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
