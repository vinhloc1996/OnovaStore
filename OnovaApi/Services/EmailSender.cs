using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public Task SendEmailResetPasswordAsync(string email, string link, string fullname)
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
            return client.SendEmailAsync(msg);
        }

        public Task SendEmailProductAvailableAsync(string link, Product product, List<EmailAddress> emails) //change model product to short and contain url product image
        {
            var key = _configuration.GetSection("SendGridService:Key").Value;
            var client = new SendGridClient(key);
            var personalize = new Personalization
            {
                Substitutions = new Dictionary<string, string>
                {
                    {":productName", product.Name},
                    {":productLink", product.Slug},
                    {":productImage", product.ProductThumbImage.ToString()} // shoud be changed, using automapper to select thumbup image
                },
                Tos = emails
            };

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@onova.com", "Support Onova"),
                Subject = "Product available: :productName",
                TemplateId = "", //new template for product available
                Personalizations = new List<Personalization> { personalize }
            };

            return client.SendEmailAsync(msg);
        }

        //one more function for customer after billing the order, will be implemented after stripe
    }
}