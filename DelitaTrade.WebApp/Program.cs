using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Configuration.AddUserSecrets(Assembly.GetEntryAssembly() ?? throw new ArgumentException("Unable to get entry assembly"));
            builder.Services.AddApplicationDatabase(builder.Configuration);
            builder.Services.AddRazorPages();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            builder.Services.AddWebApplicationIdentity();
            builder.Services.AddApplicationConfigurationManager();
            builder.Services.AddApplicationExporterServices();


            builder.Services.AddControllersWithViews();

            builder.Services.AddApplicationServices();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                string[] roles = { Admin, Driver, WarehouseManager };
                foreach (var role in roles)
                {
                    if (await roleManager.RoleExistsAsync(role) == false)
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    }
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DelitaUser>>();
                var aleks = await userManager.FindByNameAsync("AleksandrDobrov");
                if (aleks != null && await userManager.IsInRoleAsync(aleks, Driver) == false)
                {
                    await userManager.AddToRoleAsync(aleks, Driver);
                }
                var admin = await userManager.FindByNameAsync("AdminDelita");
                if (admin != null && await userManager.IsInRoleAsync(admin, Admin) == false)
                {
                    await userManager.AddToRoleAsync(admin, Admin);
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
