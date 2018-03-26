using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
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
            var key = Encoding.UTF8.GetBytes(Configuration.GetSection("Authentication:Jwt:Key").Value);
            services.AddCors();
            services.AddDbContext<OnovaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper();
            services.AddScoped<IAuthRepository, AuthRepository>();
//            services.Configure<SendGridService>(Configuration);

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.User.RequireUniqueEmail = true;
//                    options.Tokens.PasswordResetTokenProvider = "OnovaPasswordResetToken";

                    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                    options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.NameId;
                    options.ClaimsIdentity.UserNameClaimType = JwtRegisteredClaimNames.Email;
                })
                .AddEntityFrameworkStores<OnovaContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(
                    options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false
                    };
                });

//            services.Configure<SecurityStampValidatorOptions>(
//                options =>
//                {
//                    options.ValidationInterval = TimeSpan.FromSeconds(10);
//                });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/api/auth/AccessDenied";
                options.LoginPath = "/api/auth/login";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Administrator"));
                options.AddPolicy("Admin Only", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("CustomerSupport", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "CustomerSupport"));
                options.AddPolicy("ProductManager", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "ProductManager"));
                options.AddPolicy("Shipper", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Shipper"));
            });

            services.AddMvc()
                .AddJsonOptions(
                    opt =>
                    {
                        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, OnovaContext dbContext)
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
            //Run only in first-run
//            Seed.SeedData(userManager, roleManager, dbContext).Wait();
            app.UseMvc();
        }
    }
}