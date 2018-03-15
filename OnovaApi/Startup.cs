using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnovaApi.Data;
using OnovaApi.Helpers;
using OnovaApi.Models.IdentityModels;
using OnovaApi.Services;

namespace OnovaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Authentication:Jwt:Key").Value);
            services.AddCors();
            services.AddDbContext<OnovaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper();
//            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.User.RequireUniqueEmail = true;
                    options.Tokens.PasswordResetTokenProvider = "OnovaPasswordResetToken";
                })
                .AddEntityFrameworkStores<OnovaContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            }).AddFacebook(fbOptions =>
            {
                fbOptions.AppId = Configuration["ExternalLogin:Facebook:AppID"];
                fbOptions.AppSecret = Configuration["ExternalLogin:Facebook:AppSecret"];
                fbOptions.SaveTokens = true;
//                fbOptions.Scope.Add("user_birthday");
                fbOptions.Scope.Add("public_profile");
                fbOptions.Fields.Add("email");
                fbOptions.Fields.Add("picture");
                fbOptions.Fields.Add("name");
                fbOptions.Fields.Add("gender");
                fbOptions.Fields.Add("id");
            });

            services.AddMvc()
                .AddJsonOptions(
                    opt =>
                    {
                        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(e =>
                {
                    e.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}