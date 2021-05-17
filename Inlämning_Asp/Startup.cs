using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inlämning_Asp.Data;
using Microsoft.AspNetCore.Identity;
using Inlämning_Asp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace Inlämning_Asp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages(o =>
            {
                o.Conventions.AuthorizeFolder("/");
                o.Conventions.AllowAnonymousToFolder("/User");
                // Jag sätter en [AllowAnonymous] direkt i Index sidan för att demonstrera
                // o.Conventions.AllowAnonymousToPage("/Index");

                o.Conventions.AuthorizeFolder("/Admin", "RequireAdministratorRole");
                o.Conventions.AuthorizeFolder("/Organizer", "RequireOrganizerRole");

                o.Conventions.AuthorizeFolder("/Admin/SuperAdmin", "SuperAdmin");
            });

            services.AddDbContext<EventDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EventDbContext")));

            services.AddDefaultIdentity<Buyer>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvxyz";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;

                options.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<EventDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/NotAllowed";
                options.LoginPath = "/User/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;

                options.Cookie.HttpOnly = true;

                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
            });

          

            services.AddAuthorization(o =>
            {
                o.AddPolicy("RequireAdministratorRole", b =>
                {
                    b.RequireRole("Admin");
                });
                o.AddPolicy("RequireOrganizerRole", b =>
                {
                    b.RequireRole("Organizer");
                });

                o.AddPolicy("SuperAdmin", b =>
                {
                    b.RequireAuthenticatedUser();
                    b.RequireRole("Admin");
                    b.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
                        "superadmin@hotmail.com",
                        "superadmin1@hotmail.com",
                        "superadmin2@hotmail.com");
                });

                o.AddPolicy("OrganizerOwnsEvent", b =>
                {
                    b.RequireRole("Organizer");
                    //b.Requirements.Add(new EventOwnershipRequirement());
                });
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}