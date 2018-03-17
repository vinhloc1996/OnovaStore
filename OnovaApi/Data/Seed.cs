﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Data
{
    public static class Seed
    {
        public static async Task SeedRoles
            (RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
                ("CustomerSupport").Result)
            {
                ApplicationRole role = new ApplicationRole {Name = "CustomerSupport"};
                await roleManager.CreateAsync(role);
            }

            if (!roleManager.RoleExistsAsync
                ("Administrator").Result)
            {
                ApplicationRole role = new ApplicationRole {Name = "Administrator"};
                await roleManager.CreateAsync(role);
            }

            if (!roleManager.RoleExistsAsync
                ("ProductManager").Result)
            {
                ApplicationRole role = new ApplicationRole {Name = "ProductManager"};
                await roleManager.CreateAsync(role);
            }

            if (!roleManager.RoleExistsAsync
                ("Shipper").Result)
            {
                ApplicationRole role = new ApplicationRole {Name = "Shipper"};
                await roleManager.CreateAsync(role);
            }
        }

        public static async Task SeedStaffs(OnovaContext dbContext)
        {
            if (await dbContext.Staff.CountAsync() == 0)
            {
                var admin = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "admin@onova.com");
                var staff = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "staffsupport@onova.com");

                var staffInits = new List<Staff>
                {
                    new Staff
                    {
                        StaffId = admin.Id,
                        AddBy = null,
                        AddDate = DateTime.Now,
                        Address = "Onova",
                        Phone = "11111111",
                        Salary = 0
                    },
                    new Staff
                    {
                        StaffId = staff.Id,
                        AddBy = admin.Id,
                        AddDate = DateTime.Now,
                        Address = "Onova",
                        Phone = "22222222",
                        Salary = 0
                    }
                };
                await dbContext.Staff.AddRangeAsync(staffInits);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task SeedUsers
            (UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync
                    ("admin@onova.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@onova.com",
                    Email = "admin@onova.com",
                    FullName = "Admin"
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "123456").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }


            if (userManager.FindByNameAsync
                    ("staffsupport@onova.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "staffsupport@onova.com",
                    Email = "staffsupport@onova.com",
                    FullName = "Staff Test"
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "123456").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "CustomerSupport");
                }
            }
        }

        public static async Task SeedData
        (UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, OnovaContext context)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await SeedStaffs(context);
        }
    }
}