using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OnovaApi.Services
{
    public interface IEmailSender
    {
        Task<Response> SendEmailResetPasswordAsync(string email, string link, string fullname);

        Task<Response> SendEmailProductAvailableAsync(GetProductEmailDTO dto, List<EmailAddress> emails);
    }
}
