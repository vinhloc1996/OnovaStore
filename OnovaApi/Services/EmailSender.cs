using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public Task SendEmailAsync(string email, string subject, string link, string fullname)
        {
            var key = _configuration.GetSection("SendGridService:Key").Value;
            return Execute(key, subject, link, email, fullname);
        }

        public Task Execute(string apiKey, string subject, string link, string email, string fullname)
        {
            var client = new SendGridClient(apiKey);
            var personalize = new Personalization();
            personalize.Substitutions = new Dictionary<string, string>
            {
                {":userFullname", fullname },
                {":resetLink", link }
            };

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@onova.com", "Support Onova"),
                Subject = subject,
                TemplateId = "f5393e0a-75d7-4f1c-9a9c-8d1f36590509",
                Personalizations = new List<Personalization> { personalize}
//                Sections = new Dictionary<string, string>
//                {
//                    {":userFullname", fullname },
//                    {":resetLink", link }
//                }
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
