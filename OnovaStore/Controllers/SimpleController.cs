using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnovaStore.Helpers;
using OnovaStore.Models;

namespace OnovaStore.Controllers
{
    public class SimpleController : Controller
    {
        private readonly IConfiguration _configuration;
        private static readonly HttpClient Client = new HttpClient();

        public SimpleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginViaFacebook([FromQuery] string access_token)
        {
//            var access_token = HttpContext.Request.Query["access_token"];
            var client_id = _configuration.GetSection("ExternalLogin:Facebook:AppID").Value;
            var app_secret = _configuration.GetSection("ExternalLogin:Facebook:AppSecret").Value;
            var appAccessTokenResponse =
                await Client.GetStringAsync(
                    $"https://graph.facebook.com/oauth/access_token?client_id={client_id}&client_secret={app_secret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

            var userAccessTokenValidationResponse =
                await Client.GetStringAsync(
                    $"https://graph.facebook.com/debug_token?input_token={access_token}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation =
                JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return View(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
            }

            var userInfoResponse =
                await Client.GetStringAsync(
                    $"https://graph.facebook.com/v2.8/me?fields=id,email,name,gender,locale,birthday,picture&access_token={access_token}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            dynamic userExisted = Extensions.JsonDataFromApi($"api/auth/CheckUserExisted?username={userInfo.Email}",
                "get");

            if (!userExisted.isExisted)
            {
                //Display the view with password input field
                //userInfo.Password = View(...)

                return View(LoginFacebookCallback(userInfo));
            }
            return
                View(Errors.AddErrorToModelState("login_failure", "Email is already existed in database", ModelState));
        }

        [HttpPost]
        public IActionResult LoginFacebookCallback(FacebookUserData model)
        {
            if (model?.Password != null)
            {
                StringContent userFb = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                    "application/json");

                dynamic result = Extensions.JsonDataFromApi($"api/auth/FacebookLogin", "post", userFb);

                return View(ReturnAccessToken(result.access_token));
            }
            return View();
        }

        [HttpPost]
        public IActionResult ReturnAccessToken(string token)
        {
            return View();
        }
    }
}