using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Extensions;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // Register Built-In MVC Services to the Container

            services.AddDbContext<AppDbContext>(options =>
            {
                options/*.UseLazyLoadingProxies()*/.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }/*, ServiceLifetime.Scoped*/);

            //ApplicationServicesExtensions.AddApplicationServices(services); // Static Method
            services.AddApplicationServices(); // Extension Method 

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<ApplicationUser>>();
            //services.AddIdentity<ApplicationUser, IdentityRole>();
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 2;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequiredLength = 5;
                config.Password.RequireUppercase = true;
                config.Password.RequireLowercase = true;

                config.User.RequireUniqueEmail = true;
                //config.User.AllowedUserNameCharacters

                config.Lockout.MaxFailedAccessAttempts = 3;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                config.Lockout.AllowedForNewUsers = true;
            })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme /*"Cookies"*/)
            //        .AddCookie("Hamda", config =>
            //        {
            //            config.LoginPath = "/Account/SignIn";
            //            config.AccessDeniedPath = "/Home/Error";
            //        });


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
