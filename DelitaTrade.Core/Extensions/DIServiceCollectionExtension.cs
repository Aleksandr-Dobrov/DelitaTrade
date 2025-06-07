using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.Stores;
using System.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationDatabase(this IServiceCollection service, IConfiguration configuration, string connectionStringSection = "DefaultConnection")
        {
            service.AddDbContext<DelitaDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(connectionStringSection) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

            return service;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<DelitaUser, IdentityRole<Guid>>(options =>
            {
                options.ApplicationIdentityConfiguration();
            })
                .AddApplicationIdentityServices();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, DelitaRepository>()
                .AddScoped<ICompanyService, CompanyService>()
                .AddScoped<ICompanyObjectService, CompanyObjectService>()
                .AddScoped<IReturnProtocolService, ReturnProtocolService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductDescriptionService, ProductDescriptionService>()
                .AddScoped<IReturnProductService, ReturnProductService>()
                .AddScoped<ITraderService, TraderService>()
                .AddScoped<IVehicleService, VehicleService>()
                .AddScoped<IInvoiceInDayReportService, InvoiceInDayReportService>()
                .AddScoped<IDayReportService, DayReportService>()
                .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
                .AddScoped<IBanknotesService, BanknotesService>()
                .AddScoped<IDescriptionCategoryService, DescriptionCategoryService>();

            return services;
        }

        public static IServiceCollection AddApplicationExporterServices(this IServiceCollection services)
        {

            services.AddScoped<IReturnProtocolBuildersStore, ReturnProtocolBuildersStore>()
                .AddScoped<IReturnProtocolBuilderService, ReturnProtocolBuilderService>()
                .AddTransient<ExcelReturnProtocolBuilder>()
                .AddTransient<ExcelCrisisReturnProtocolBuilder>()
                .AddTransient<ExcelStupidReturnProtocolBuilder>()
                .AddTransient<ExcelStupidCardReturnProtocolBuilder>();

            return services;
        }

        public static IServiceCollection AddApplicationConfigurationManager(this IServiceCollection services)
        {
            services.AddSingleton(System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

            return services;
        }
    }
}
