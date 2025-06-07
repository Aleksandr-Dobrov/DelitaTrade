using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DelitaTrade.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<DelitaDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddRazorPages();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<DelitaUser, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
                .AddUserManager<UserManager<DelitaUser>>()
                .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                .AddSignInManager<SignInManager<DelitaUser>>()
                .AddUserStore<UserStore<DelitaUser, IdentityRole<Guid>, DelitaDbContext, Guid>>()
                .AddEntityFrameworkStores<DelitaDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IRepository, DelitaRepository>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<ICompanyObjectService, CompanyObjectService>();
            builder.Services.AddScoped<IReturnProtocolService, ReturnProtocolService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductDescriptionService, ProductDescriptionService>();
            builder.Services.AddScoped<IReturnProductService, ReturnProductService>();
            builder.Services.AddScoped<ITraderService, TraderService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IInvoiceInDayReportService, InvoiceInDayReportService>();
            builder.Services.AddScoped<IDayReportService, DayReportService>();
            builder.Services.AddScoped<IInvoicePaymentService, InvoicePaymentService>();
            builder.Services.AddScoped<IBanknotesService, BanknotesService>();
            builder.Services.AddScoped<IDescriptionCategoryService, DescriptionCategoryService>();

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
