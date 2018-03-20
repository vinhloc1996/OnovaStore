using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using Newtonsoft.Json;
using OnovaApi.DTOs;
using OnovaApi.Helpers;
using OnovaApi.Models.DatabaseModels;
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
        private static readonly HttpClient Client = new HttpClient();

        public AuthController(IConfiguration configuration, IAuthRepository repository)
        {
//            _userManager = userManager;
//            _signInManager = signInManager;
            _configuration = configuration;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return StatusCode(403, "You don't have privilege to access this page!");
        }


        [Authorize(Policy = "Admin")]
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff([FromBody] StaffInfoDTO staffInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.AddStaff(staffInfoDto, User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (result.Succeeded)
            {
                return StatusCode(201, "Staff has been added successfully!");
            }

            throw new Exception("Add new user failed");
        }

        [AllowAnonymous]
        [HttpGet("CheckUserExisted")]
        public async Task<IActionResult> CheckUserExisted([FromQuery] string username)
        {
            var result = await _repository.FindUserByUserName(username) != null;

            return Json(new {isExisted = result});
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDTO userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.UserRegister(userForRegisterDto);

            if (result.Succeeded)
            {
                return StatusCode(201, "Customer has been created successfully!");
            }

            throw new Exception("User register failed");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO userForLoginDto)
        {
            var user = await _repository.FindUserByUserName(userForLoginDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _repository.LoginSucceeded(userForLoginDto);

            if (result.Succeeded)
            {
                var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Jwt:Key").Value);

                return Ok(await _repository.GenerateJwtToken(user, key));
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("FacebookLogin")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookUserData userData)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Jwt:Key").Value);

            var appUser = new ApplicationUser()
            {
                FullName = userData.Name,
                Email = userData.Email,
                UserName = userData.Email,
                Gender = userData.Gender.ToLower() == "male",
            };

            //Process in mvc controller, if login facebook success then return user info in json with password.
            
            var result = await _repository.CreateUser(appUser, userData.Password);

            if (!result.Succeeded)
                return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _repository.AddCustomer(new Customer
            {
                CustomerId = appUser.Id,
                JoinDate = DateTime.Now,
                FacebookId = userData.Id
            });
            
            var localUser = await _repository.FindUserByUserName(userData.Email);
            
            return Ok(await _repository.GenerateJwtToken(localUser, key));
        }
    }
}