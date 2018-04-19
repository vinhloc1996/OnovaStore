using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnovaApi.DTOs;
using OnovaApi.Models.DatabaseModels;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Services
{
    public interface IAuthRepository
    {
        Task<SignInResult> LoginSucceeded(UserForLoginDTO userForLoginDto);
        Task<IList<string>> UserRoles(ApplicationUser user);
        Task<List<Claim>> InitClaims(ApplicationUser user);
        Task<ApplicationUser> FindUserById(string id);
        Task<ApplicationUser> FindUserByUserName(string username);
        Task<IdentityResult> AddStaff(StaffInfoDTO staff, string adminId);
        Task<IdentityResult> UserRegister(UserForRegisterDTO dto);
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);
        Task<int> AddCustomer(Customer customer);
        Task<object> GenerateJwtToken(ApplicationUser user, byte[] key);
        Task<Customer> CurrentCustomer(string userId);
        Task<string> PasswordResetToken(ApplicationUser user);
        Task SendEmailPasswordReset(string email, string callBackUrl, string fullname);
        Task<IdentityResult> ResetPassword(ApplicationUser user, string code, string newPassword);
        Task MoveCart(string anonymousId, string email);
    }
}