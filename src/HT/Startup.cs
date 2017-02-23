using HT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HT
{
    public class Startup
    {
        public static readonly CookieAuthenticationOptions CookieAuthenticationOptions = new CookieAuthenticationOptions
        {
            AuthenticationScheme = "MyCookieMiddlewareInstance",
            LoginPath = new PathString("/Login/Login/"),
            AccessDeniedPath = new PathString("/Account/Forbidden/"),
            AutomaticAuthenticate = true,
            AutomaticChallenge = true
        };

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            string constr = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
            if (constr != null)
            {
                services.AddDbContext<HarmonyToysDatabaseContext>(options => options.UseSqlServer(constr));
            }
            else
            {
                services.AddDbContext<HarmonyToysDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policy => policy.RequireRole("Admin"));
            });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, HarmonyToysDatabaseContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseCookieAuthentication(CookieAuthenticationOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(context);
        }
    }
}
