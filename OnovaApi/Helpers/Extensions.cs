using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OnovaApi.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static byte[] KeyJwt(IConfiguration configuration)
        {
            return Encoding.UTF8.GetBytes(configuration.GetSection("Authentication:Jwt:Key").Value);
        }
    }
}