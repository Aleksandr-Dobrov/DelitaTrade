using DelitaTrade.WebApp.Extensions;
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
            builder.Services.AddApplicationDatabase(builder.Configuration, "DelitaConnection");
            builder.Services.AddRazorPages();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            builder.Services.AddWebApplicationIdentity();
            builder.Services.AddApplicationConfigurationManager();
            builder.Services.AddApplicationExporterServices();


            builder.Services.AddControllersWithViews();

            builder.Services.AddApplicationServices();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    {
                        policy.WithOrigins("https://kit.fontawesome.com")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                        //TODO: check if this is correct solution for missing FontAwesome icons
                    }
                });
            });

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

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.SeedingIdentityData();

            app.UseRouting();

            app.UseAuthorization();
 
            app.UseAdminRedirection();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
