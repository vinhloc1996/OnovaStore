using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnovaApi.Data;
using OnovaApi.DTOs;
using OnovaApi.Models.IdentityModels;

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

        public async Task<ApplicationUser> UserExisted(string username) => await _userManager.FindByNameAsync(username);

        public async Task<SignInResult> LoginSucceeded(UserForLoginDTO userForLoginDto)
            =>
                _signInManager.CheckPasswordSignInAsync(await UserExisted(userForLoginDto.Email),
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
    }
}