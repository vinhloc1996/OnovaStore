using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class User
    {
        public User()
        {
            UserClaim = new HashSet<UserClaim>();
            UserLogin = new HashSet<UserLogin>();
            UserRole = new HashSet<UserRole>();
            UserToken = new HashSet<UserToken>();
        }

        public string UserId { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        public UserProfile UserProfile { get; set; }
        public ICollection<UserClaim> UserClaim { get; set; }
        public ICollection<UserLogin> UserLogin { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
        public ICollection<UserToken> UserToken { get; set; }
    }
}
