using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnovaStore.Helpers;
using OnovaStore.Models;
using OnovaStore.Models.Account;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private static readonly HttpClient Client = new HttpClient();

        public AccountController(IClaimPrincipalManager claimPrincipalManager, IConfiguration configuration)
        {
            _claimPrincipalManager = claimPrincipalManager;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(String returnUrl = null)
        {
            if (_claimPrincipalManager.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _claimPrincipalManager.LogoutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model, String returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Invalid login form");
                return View(model);
            }

            if (await _claimPrincipalManager.LoginAsync(model.Email, model.Password))
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError(String.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(String returnUrl = null)
        {
            if (_claimPrincipalManager.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model, String returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Invalid register form");
                return View(model);
            }

            dynamic user =
                    await Extensions.JsonDataFromApi($"api/auth/Register",
                        "post", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                            "application/json"));
            if (user.result == true)
            {
                if (await _claimPrincipalManager.LoginAsync(model.Email, model.Password))
                    return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(String.Empty, user.message.ToString());
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAccessToken()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginViaFacebook([FromQuery] string accessToken, [FromQuery] bool status,
            [FromQuery] string error, [FromQuery] string errorDescription)
        {
            if (status)
            {
                var client_id = _configuration.GetSection("ExternalLogin:Facebook:AppID").Value;
                var app_secret = _configuration.GetSection("ExternalLogin:Facebook:AppSecret").Value;

                var appAccessTokenResponse =
                    await Client.GetStringAsync(
                        $"https://graph.facebook.com/oauth/access_token?client_id={client_id}&client_secret={app_secret}&grant_type=client_credentials");
                var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

                var userAccessTokenValidationResponse =
                    await Client.GetStringAsync(
                        $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appAccessToken.AccessToken}");
                var userAccessTokenValidation =
                    JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

                if (!userAccessTokenValidation.Data.IsValid)
                {
                    return View("LoginViaFacebook", "Access token is invalid");
                }

                var userInfoResponse =
                    await Client.GetStringAsync(
                        $"https://graph.facebook.com/v2.8/me?fields=id,email,name,gender,locale,birthday,picture&access_token={accessToken}");

                var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

                dynamic userExisted =
                    await Extensions.JsonDataFromApi($"api/auth/CheckUserExistedForFbLogin",
                        "post", new StringContent(userInfoResponse, Encoding.UTF8,
                            "application/json"));

                if (userExisted.isExisted == false)
                {
                    TempData.Put("userInfo", userInfo);
                    return RedirectToAction("LoginFacebookCallback");
                }

                if (await _claimPrincipalManager.LoginFbAsync(userExisted.data.access_token.ToString()))
                    return RedirectToAction("Index", "Home");
            }

            return View("LoginViaFacebook", "Unthorization error, user cancel");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginFacebookCallback()
        {
            var model = TempData.Get<FacebookUserData>("userInfo");
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginFacebookCallback(FacebookUserData model, String returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                StringContent userFb = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                    "application/json");

                dynamic result = await Extensions.JsonDataFromApi($"api/auth/FacebookLogin", "post", userFb);

                if (await _claimPrincipalManager.LoginFbAsync(result.access_token.ToString())) 
                    return RedirectToLocal(returnUrl);

                return View("LoginViaFacebook", "Login failed!");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                StringContent email = new StringContent(JsonConvert.SerializeObject(model.Email), Encoding.UTF8,
                    "application/json");

                dynamic data = await Extensions.JsonDataFromApi($"api/auth/ForgotPasswordToken", "post", email);

                if (data.result == false)
                {
                    ModelState.AddModelError(String.Empty, data.message.ToString());
                    return View(model);
                }

                string userId = data.userId.ToString();
                string token = data.token.ToString();

                model.CallbackUrl = Url.ResetPasswordCallbackLink(userId, token, Request.Scheme);

                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                    "application/json");
                dynamic forgotPassword = await Extensions.JsonDataFromApi($"api/auth/ForgotPassword", "post", content);

                if (forgotPassword.result == true)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                ModelState.AddModelError(String.Empty, forgotPassword.message.ToString());
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string code = null, [FromQuery] string userId = null)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId))
            {
                throw new ApplicationException("A code and userId must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel
            {
                Id = userId,
                Code = code
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                    "application/json");
                dynamic resetPassword = await Extensions.JsonDataFromApi($"api/auth/ResetPassword", "post", content);

                if (resetPassword.result == true)
                {
                    return RedirectToAction("ResetPasswordSuccess");
                }

                ModelState.AddModelError(String.Empty, resetPassword.message.ToString());
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        private IActionResult RedirectToLocal(String returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}