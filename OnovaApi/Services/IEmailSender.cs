using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnovaApi.Models.DatabaseModels;
using SendGrid.Helpers.Mail;

namespace OnovaApi.Services
{
    public interface IEmailSender
    {
        Task SendEmailResetPasswordAsync(string email, string link, string fullname);

        Task SendEmailProductAvailableAsync(string link, Product product, List<EmailAddress> emails);
    }
}
