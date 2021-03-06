﻿using System;
using System.Collections.Generic;
using System.Config;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnovaStore.Helpers;
using Stripe;

namespace OnovaStore
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
            var value = Configuration.GetSection("JwtTokenValidationSettings:SecretKey").Value;
            var key = Encoding.UTF8.GetBytes(value);
            services.AddCors();
            // Setup REST client
            services.Configure<RestClientSettings>(Configuration.GetSection(nameof(RestClientSettings)));
            services.AddTransient<IRestClient, RestClientFactory>();

            // setup JWT Token validation
            services.Configure<JwtTokenValidationSettings>(Configuration.GetSection(nameof(JwtTokenValidationSettings)));
            services.AddSingleton<IJwtTokenValidationSettings, JwtTokenValidationSettingsFactory>();

            // Setup JWT Issuer Settings
            services.Configure<JwtTokenIssuerSettings>(Configuration.GetSection(nameof(JwtTokenIssuerSettings)));
            services.AddSingleton<IJwtTokenIssuerSettings, JwtTokenIssuerSettingsFactory>();

            // Setup ClaimPrincipalManager
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IClaimPrincipalManager, ClaimPrincipalManager>();

            // Setup Authentication Cookies
            services.Configure<AuthenticationSettings>(Configuration.GetSection(nameof(AuthenticationSettings)));
            services.AddSingleton<IAuthenticationSettings, AuthenticationSettingsFactory>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            var serviceProvider = services.BuildServiceProvider();
            var authenticationSettings = serviceProvider.GetService<IAuthenticationSettings>();
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = authenticationSettings.LoginPath;
                        options.AccessDeniedPath = authenticationSettings.AccessDeniedPath;
                    }
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Admin Only", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
                options.AddPolicy("Staff Only", policy => policy.RequireRole("CustomerSupport", "Administrator", "ProductManager", "Shipper"));
            });

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/NotFound404";
                    await next();
                }
            });

            app.UseStaticFiles();
            app.UseSession();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "ViewProduct",
                    template: "product/{slug}",
                    defaults: new { Areas = "", controller = "Product", action = "Detail"}
                );

                routes.MapRoute(
                    name: "ViewCategory",
                    template: "category/{slug}",
                    defaults: new { Areas = "", controller = "Category", action = "List" }
                );

                routes.MapRoute(
                    name: "ViewBrand",
                    template: "brand/{slug}",
                    defaults: new {Areas = "", controller = "Brand", action = "List" }
                );
            });
        }
    }
}