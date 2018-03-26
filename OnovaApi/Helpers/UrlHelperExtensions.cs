using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnovaApi.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
//        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
//        {
//            return urlHelper.Action(
//                action: nameof(AuthController.ConfirmEmail),
//                controller: "Account",
//                values: new { userId, code },
//                protocol: scheme);
//        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AuthController.ResetPassword),
                controller: "Auth",
                values: new {userId, code},
                protocol: scheme);
        }
    }
}
