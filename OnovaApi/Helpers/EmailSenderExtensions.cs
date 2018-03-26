using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using OnovaApi.Services;

namespace OnovaApi.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link, string fullname)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                HtmlEncoder.Default.Encode(link), fullname);
        }
    }
}
