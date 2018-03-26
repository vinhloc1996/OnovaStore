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
using Microsoft.WindowsAzure.Storage.File;
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
        private readonly IConfiguration _configuration;

        private readonly IAuthRepository _repository;

        public AuthController(IConfiguration configuration, IAuthRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return StatusCode(403,
                new {message = "You don't have privilege to access this page!"});
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
        [HttpPost("CheckUserExistedForFbLogin")]
        public async Task<IActionResult> CheckUserExistedForFbLogin([FromBody] FacebookUserData model)
        {
            var user = await _repository.FindUserByUserName(model.Email);
            bool userExisted = user != null;

            if (userExisted)
            {
                var customer = await _repository.CurrentCustomer(user.Id);

                if (customer.FacebookId == model.Id)
                {
                    return
                        Json(
                            new
                            {
                                isExisted = userExisted,
                                data = await _repository.GenerateJwtToken(user, Extensions.KeyJwt(_configuration))
                            });
                }
            }

            return Json(new {isExisted = userExisted});
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDTO userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExisted = await _repository.FindUserByUserName(userForRegisterDto.Email);

            if (userExisted == null)
            {
                var results = await _repository.UserRegister(userForRegisterDto);
                if (results.Succeeded)
                {
                    return Json(new {result = results.Succeeded, message = "Customer has been signed up successful"});
                }

                return Json(new {result = results.Succeeded, message = "Error while signing up customer, try again"});
            }

            return Json(new {result = userExisted != null, message = "User already existed in the system"});
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
                var key = Extensions.KeyJwt(_configuration);

                return Ok(await _repository.GenerateJwtToken(user, key));
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPasswordToken")]
        public async Task<IActionResult> ForgotPasswordToken([FromBody] string email)
        {
            var user = await _repository.FindUserByUserName(email);

            if (user == null)
            {
                return Json(new {result = false, message = "The user doesn't exist in system"});
            }

            var code = await _repository.PasswordResetToken(user);

            return Json(new {result = true, userId = user.Id, token = code});
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPassword model)
        {
            var user = await _repository.FindUserByUserName(model.Email);

            if (user != null)
            {
                await _repository.SendEmailPasswordReset(model.Email, model.CallbackUrl);

                return Json(new { result = true, message = "The reset password link was sent." });
            }

            return Json(new {result = false, message = "Email doesn't exist in the system"});
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPassword model)
        {
            var user = await _repository.FindUserById(model.Id);
            var results = false;

            if (user != null)
            {
                results = await _repository.ResetPassword(user, model.Code, model.Password) == IdentityResult.Success;

                if (results)
                {
                    return Json(new { result = results, message = "The password has been resetted successful." });
                }
            }

            return Json(new {result = results, message = "The password cannot reset, invalid user!"});
        }

        [AllowAnonymous]
        [HttpPost("FacebookLogin")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookUserData userData)
        {
            var key = Extensions.KeyJwt(_configuration);

            var appUser = new ApplicationUser()
            {
                FullName = userData.Name,
                Email = userData.Email,
                UserName = userData.Email,
                Gender = userData.Gender.ToLower() == "male",
                Picture = userData.Picture.Data.Url
            };

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