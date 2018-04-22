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
        Task<Response> SendEmailOrderSuccessfulAsync(string orderNumber, double? subTotal, double? discount, double? shipping, double? tax, double? total, string fullName, string phone, string addressLine1, string city, string zip, string email);
    }
}
