using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Models.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OnovaApi.Helpers;

namespace OnovaApi.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OnovaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthRepository(OnovaContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<object> GenerateJwtToken(ApplicationUser user, byte[] key)
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken
            (
                claims: await InitClaims(user),
                signingCredentials:
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new {access_token = tokenHandle.WriteToken(token)};
        }

        public async Task<ApplicationUser> FindUserById(string id) => await _userManager.FindByIdAsync(id);

        public async Task<IdentityResult> ChangePassword(ApplicationUser user, string currentPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task<ApplicationUser> FindUserByUserName(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<SignInResult> LoginSucceeded(UserForLoginDTO userForLoginDto)
            =>
                _signInManager.CheckPasswordSignInAsync(await FindUserByUserName(userForLoginDto.Email),
                    userForLoginDto.Password, false).Result;

        public async Task<IList<string>> UserRoles(ApplicationUser user) => await _userManager.GetRolesAsync(user);

        public async Task<List<Claim>> InitClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddDays(1).ToUnixEpochDate().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUnixEpochDate().ToString(),
                    ClaimValueTypes.Integer64)
            };

            foreach (var userRole in await UserRoles(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return claims;
        }

        public async Task<IdentityResult> AddStaff(StaffInfoDTO staff, string adminId)
        {
            if (await FindUserByUserName(staff.Email) == null)
            {
                var newStaff = new ApplicationUser
                {
                    UserName = staff.Email,
                    Email = staff.Email,
                    FullName = staff.FullName
                };

                var result = await _userManager.CreateAsync(newStaff, staff.Password);

                if (result.Succeeded)
                {
                    var newUser = await FindUserByUserName(staff.Email);
                    var currentAdmin = adminId;

                    await _context.Staff.AddAsync(new Staff
                    {
                        StaffId = newUser.Id,
                        AddBy = currentAdmin,
                        AddDate = staff.AddDate,
                        Address = staff.Address,
                        Phone = staff.Phone,
                        Salary = staff.Salary
                    });
                    await _context.SaveChangesAsync();

                    return await _userManager.AddToRoleAsync(newStaff, staff.Role);
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UserRegister(UserForRegisterDTO dto)
        {
            var createNewCustomer = await CreateUser(new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FullName = dto.FullName
            }, dto.Password);

            if (createNewCustomer.Succeeded)
            {
                var newUser = await FindUserByUserName(dto.Email);

                var result = await AddCustomer(new Customer
                {
                    CustomerId = newUser.Id,
                    JoinDate = dto.JoinDate
                });

                await _context.SaveChangesAsync();

                return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
            }

            return IdentityResult.Failed();
        }

        public async Task<int> AddCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            _context.CustomerCart.Add(
                new CustomerCart {CustomerCartId = customer.CustomerId, CreateDate = DateTime.Now});

            return await _context.SaveChangesAsync();
        }

        public async Task<Customer> CurrentCustomer(string userId)
        {
            return await _context.Customer.FindAsync(userId);
        }

        public async Task<string> PasswordResetToken(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task SendEmailPasswordReset(string email, string callBackUrl, string fullname)
        {
            await _emailSender.SendEmailResetPasswordAsync(email, callBackUrl, fullname);
        }

        public async Task<IdentityResult> ResetPassword(ApplicationUser user, string code, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, code, newPassword);
        }

        public async Task<IdentityResult> AdminResetPassword(ApplicationUser user, string newPassword)
        {
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, newPassword);
            _context.Users.Update(user);

            return await _context.SaveChangesAsync() > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task MoveCart(string anonymousId, string email)
        {
            if (!string.IsNullOrEmpty(anonymousId))
            {
                var anonymousCart =
                    _context.AnonymousCustomerCartDetail.Where(c => c.AnonymousCustomerCartId == anonymousId);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                var customerCart = _context.CustomerCart.Find(user.Id);

                if (anonymousCart.Any())
                {
                    foreach (var item in anonymousCart.ToList())
                    {
                        var currItemInCart =
                            _context.CustomerCartDetail.Find(customerCart.CustomerCartId, item.ProductId);
                        var product = _context.Product.Find(item.ProductId);
                        var quantityAfterMerge = item.Quantity;

                        if (currItemInCart != null)
                        {
                            quantityAfterMerge += currItemInCart.Quantity;

                            if (quantityAfterMerge <= product.MaximumQuantity &&
                                quantityAfterMerge <= product.CurrentQuantity)
                            {
                                currItemInCart.Quantity = quantityAfterMerge;
                                _context.CustomerCartDetail.Update(currItemInCart);

                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    var quantityRemain = _context.CustomerCartDetail
                                        .Where(c => c.CustomerCartId == customerCart.CustomerCartId)
                                        .Sum(c => c.Quantity);
                                    var totalPriceRemain = _context.CustomerCartDetail
                                        .Where(c => c.CustomerCartId == customerCart.CustomerCartId)
                                        .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity })
                                        .Sum(c => c.totalPriceItem);

                                    customerCart.DisplayPrice = totalPriceRemain;
                                    customerCart.TotalQuantity = quantityRemain;

                                    _context.CustomerCart.Update(customerCart);

                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            _context.CustomerCartDetail.Add(new Models.DatabaseModels.CustomerCartDetail
                            {
                                CustomerCartId = customerCart.CustomerCartId,
                                ProductId = product.ProductId,
                                DisplayPrice = product.DisplayPrice,
                                Quantity = quantityAfterMerge,
                                Price = product.RealPrice
                            });

                            await _context.SaveChangesAsync();

                            var quantity = _context.CustomerCartDetail
                                .Where(c => c.CustomerCartId == customerCart.CustomerCartId)
                                .Sum(c => c.Quantity);
                            var totalPrice = _context.CustomerCartDetail
                                .Where(c => c.CustomerCartId == customerCart.CustomerCartId)
                                .Select(c => new { totalPriceItem = c.DisplayPrice * c.Quantity })
                                .Sum(c => c.totalPriceItem);

                            customerCart.DisplayPrice = totalPrice;
                            customerCart.TotalQuantity = quantity;

                            _context.CustomerCart.Update(customerCart);

                            await _context.SaveChangesAsync();
                        }
                    }
                }

                if (!_context.Order.Any(c => c.CartId == anonymousId))
                {
                    _context.AnonymousCustomerCartDetail.RemoveRange(
                        _context.AnonymousCustomerCartDetail.Where(c =>
                            c.AnonymousCustomerCartId == anonymousId));
                    await _context.SaveChangesAsync();

                    var oldCart = _context.AnonymousCustomerCart.Find(anonymousId);
                    if (oldCart != null)
                    {
                        _context.AnonymousCustomerCart.Remove(oldCart);
                    }
                    
                    await _context.SaveChangesAsync();

                    var oldAnonymousCus = _context.AnonymousCustomer.Find(anonymousId);
                    if (oldAnonymousCus != null)
                    {
                        _context.AnonymousCustomer.Remove(oldAnonymousCus);
                    }
                    
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}