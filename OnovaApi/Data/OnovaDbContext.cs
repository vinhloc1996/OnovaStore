using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Models.DatabaseModels;

namespace OnovaApi.Data
{
    public class OnovaDbContext : IdentityDbContext<IdentityUser>
    {
        public OnovaDbContext(DbContextOptions<OnovaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            builder.Entity<IdentityUser>().Property(p => p.Id).HasColumnName("UserID");
            builder.Entity<IdentityRole>().Property(p => p.Id).HasColumnName("RoleID");
            builder.Entity<IdentityRoleClaim<string>>().Property(p => p.Id).HasColumnName("RoleClaimID");
            builder.Entity<IdentityUserClaim<string>>().Property(p => p.Id).HasColumnName("UserClaimID");
        }
    }
}