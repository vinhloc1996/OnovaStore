using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
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

        public IActionResult GetAccessToken()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginViaFacebook([FromQuery] string accessToken, [FromQuery] bool status, [FromQuery] string error, [FromQuery] string errorDescription)
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
                    return View(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
                }

                var userInfoResponse =
                    await Client.GetStringAsync(
                        $"https://graph.facebook.com/v2.8/me?fields=id,email,name,gender,locale,birthday,picture&access_token={accessToken}");
                var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

                dynamic userExisted =
                    await Extensions.JsonDataFromApi($"api/auth/CheckUserExisted?username={userInfo.Email}",
                        "get");

                if (userExisted.isExisted == false)
                {
                    TempData.Put("userInfo", userInfo);
                    //                var a = Url.Action("LoginFacebookCallback");
                    return RedirectToAction("LoginFacebookCallback");
                }
            }
            
            return
                View(Errors.AddErrorToModelState(error, errorDescription, ModelState));
        }

        [HttpGet]
        public IActionResult LoginFacebookCallback()
        {
            var model = TempData.Get<FacebookUserData>("userInfo");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginFacebookCallback(FacebookUserData model)
        {
            if (ModelState.IsValid)
            {
                StringContent userFb = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                    "application/json");

                dynamic result = await Extensions.JsonDataFromApi($"api/auth/FacebookLogin", "post", userFb);

                TempData["access_token"] = result.access_token.ToString();
                return RedirectToAction("ReturnAccessToken");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ReturnAccessToken()
        {
            var access_token = TempData["access_token"].ToString();
            return View("ReturnAccessToken", access_token);
        }
    }
}