using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace DelitaTrade.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
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
