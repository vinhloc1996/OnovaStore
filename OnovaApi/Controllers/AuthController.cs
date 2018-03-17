using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnovaApi.DTOs;
using OnovaApi.Helpers;
using OnovaApi.Models.IdentityModels;
using OnovaApi.Services;

namespace OnovaApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _repository;

        public AuthController(IConfiguration configuration, IAuthRepository repository)
        {
//            _userManager = userManager;
//            _signInManager = signInManager;
            _configuration = configuration;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff([FromBody] UserForRegisterDTO userForRegisterDto)
        {

        }

//        [AllowAnonymous]
//        [HttpPost("Register")]
//        public async Task<IActionResult> Register([FromBody] UserForRegisterDTO userForRegisterDto)
//        {
//
//        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO userForLoginDto)
        {
            var user = await _repository.UserExisted(userForLoginDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _repository.LoginSucceeded(userForLoginDto);

            if (result.Succeeded)
            {
                var tokenHandle = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Jwt:Key").Value);

                var claims = await _repository.InitClaims(user);

                var jwt = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials:
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

//                var claimIdentity = new ClaimsIdentity(claims);
//                claimIdentity.AddClaims(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
//
//                var tokenDescriptor = new SecurityTokenDescriptor
//                {
//                    Subject = claimIdentity,
//                    Expires = DateTime.Now.AddDays(1),
//                    SigningCredentials =
//                        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
//                };
//
//                var token = tokenHandle.CreateToken(tokenDescriptor);

                var tokenString = tokenHandle.WriteToken(jwt);

                return Ok(new {tokenString});
            }

            return Unauthorized();
        }
    }
}