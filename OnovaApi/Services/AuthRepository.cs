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

namespace OnovaApi.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OnovaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthRepository(OnovaContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user, byte[] key)
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken
            (
                claims: await InitClaims(user),
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials:
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return tokenHandle.WriteToken(token);
        }

        public async Task<ApplicationUser> FindUserByUserName(string username) => await _userManager.FindByNameAsync(username);

        public async Task<SignInResult> LoginSucceeded(UserForLoginDTO userForLoginDto)
            =>
                _signInManager.CheckPasswordSignInAsync(await FindUserByUserName(userForLoginDto.Email),
                    userForLoginDto.Password, false).Result;

        public async Task<IList<string>> UserRoles(ApplicationUser user) => await _userManager.GetRolesAsync(user);

        public async Task<List<Claim>> InitClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
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
            if (await FindUserByUserName(dto.Email) == null)
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

                    return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<int> AddCustomer(Customer customer)
        {
            await _context.Customer.AddAsync(customer);

            return await _context.SaveChangesAsync();
        }
    }
}