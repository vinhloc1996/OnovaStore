using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnovaApi.DTOs;
using OnovaApi.Helpers;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.Email);
            if (user == null)
            {
                HttpContext.Response.AddApplicationError("Invalid UserName!");
                return null;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var tokenHandle = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtAuthentication:Key").Value);
                var userRoles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials =
                        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
                };

                var token = tokenHandle.CreateToken(tokenDescriptor);
                var tokenString = tokenHandle.WriteToken(token);

                return Ok(new {tokenString});
            }

            HttpContext.Response.AddApplicationError("Invalid UserName or Password!");
            return null;
        }
    }
}