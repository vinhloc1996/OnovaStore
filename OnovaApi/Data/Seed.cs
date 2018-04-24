using System;
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
        public static async Task SeedProductStatus(OnovaContext context)
        {
            if (await context.ProductStatus.FirstOrDefaultAsync(p => p.StatusCode == "Available") == null)
            {
                context.ProductStatus.Add(new ProductStatus
                {
                    StatusCode = "Available",
                    StatusDescription = "Product Available",
                    StatusName = "Available"
                });
            }

            if (await context.ProductStatus.FirstOrDefaultAsync(p => p.StatusCode == "SoldOut") == null)
            {
                context.ProductStatus.Add(new ProductStatus
                {
                    StatusCode = "SoldOut",
                    StatusDescription = "Product Solde Out",
                    StatusName = "Sold Out"
                });
            }

//            if (await context.ProductStatus.FirstOrDefaultAsync(p => p.StatusCode == "OpenReserve") == null)
//            {
//                context.ProductStatus.Add(new ProductStatus
//                {
//                    StatusCode = "OpenReserve",
//                    StatusDescription = "Product Open for Reserving",
//                    StatusName = "Open Reserve"
//                });
//            }

            if (await context.ProductStatus.FirstOrDefaultAsync(p => p.StatusCode == "StopSelling") == null)
            {
                context.ProductStatus.Add(new ProductStatus
                {
                    StatusCode = "StopSelling",
                    StatusDescription = "Product Stop Selling",
                    StatusName = "Stop Selling"
                });
            }

            await context.SaveChangesAsync();
        }

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
                var manager = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "productmanager@onova.com");

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
                    },
                    new Staff
                    {
                        StaffId = manager.Id,
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

            if (userManager.FindByNameAsync
                    ("productmanager@onova.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "productmanager@onova.com",
                    Email = "productmanager@onova.com",
                    FullName = "Product Manager"
                };

                IdentityResult result = userManager.CreateAsync
                    (user, "123456").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "ProductManager");
                }
            }
        }

        public static async Task SeedOrderStatus(OnovaContext context)
        {
            if (context.OrderStatus.Find(1) == null)
            {
                context.OrderStatus.Add(new OrderStatus
                {
                    Code = "Paid",
                    Name = "Order Paid"
                });
            }

            if (context.OrderStatus.Find(2) == null)
            {
                context.OrderStatus.Add(new OrderStatus
                {
                    Code = "Cancel",
                    Name = "Order Cancel"
                });
            }

            if (context.OrderStatus.Find(3) == null)
            {
                context.OrderStatus.Add(new OrderStatus
                {
                    Code = "Refund",
                    Name = "Order Refund"
                });
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedData
        (UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, OnovaContext context)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await SeedStaffs(context);
            await SeedProductStatus(context);
            await SeedOrderStatus(context);
        }
    }
}