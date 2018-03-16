using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnovaApi.DTOs;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Services
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> UserExisted(string username);
        Task<SignInResult> LoginSucceeded(UserForLoginDTO userForLoginDto);
        Task<IList<string>> UserRoles(ApplicationUser user);
        Task<List<Claim>> InitClaims(ApplicationUser user);
    }
}