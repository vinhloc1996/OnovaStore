using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OnovaApi.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> SendEmailResetPasswordAsync(string email, string link, string fullname)
        {
            var key = _configuration.GetSection("SendGridService:Key").Value;
            var client = new SendGridClient(key);
            var personalize = new Personalization
            {
                Substitutions = new Dictionary<string, string>
                {
                    {":userFullname", fullname},
                    {":resetLink", link}
                }
            };

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@onova.com", "Support Onova"),
                Subject = "Reset Password Onova",
                TemplateId = "f5393e0a-75d7-4f1c-9a9c-8d1f36590509",
                Personalizations = new List<Personalization> {personalize}
            };
            msg.AddTo(new EmailAddress(email));
            return await client.SendEmailAsync(msg);
        }

        public async Task<Response> SendEmailProductAvailableAsync(GetProductEmailDTO dto, List<EmailAddress> emails) //change model product to short and contain url product image
        {
            var key = _configuration.GetSection("SendGridService:Key").Value;
            var client = new SendGridClient(key);
            var personalize = new Personalization
            {
                Substitutions = new Dictionary<string, string>
                {
                    {":productName", dto.Name},
                    {":productLink", "http://localhost:58212" + dto.Slug},
                    {":productImage", dto.ThumbImageUrl} // shoud be changed, using automapper to select thumbup image
                },
                Tos = emails
            };

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@onova.com", "Support Onova"),
                Subject = "Product available: :productName",
                TemplateId = "6d13c4d4-5330-42be-8d2e-021978fab05d", //new template for product available
                
                Personalizations = new List<Personalization> { personalize }
            };

            return await client.SendEmailAsync(msg);
        }

        //one more function for customer after billing the order, will be implemented after stripe
    }
}