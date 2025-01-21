using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models.MySqlDataBase;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.ViewModels;
using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using DelitaTrade.ViewModels.ReturnProtocolControllers;

namespace DelitaTrade.Models.DI
{
    public static class DIContainer
    {
        public static void BuildServiceCollection(this IServiceCollection collection, IConfiguration configuration) 
        {
            collection.AddDbContext<DelitaDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DelitaConnection")));
            collection.AddScoped<IRepository, DelitaRepository>();
            collection.AddScoped<IUserService, UserService>();
            collection.AddScoped<ICompanyService, CompanyService>();
            collection.AddScoped<ICompanyObjectService, CompanyObjectService>();
            collection.AddScoped<IReturnProtocolService, ReturnProtocolService>();
            collection.AddScoped<IProductService, ProductService>();
            collection.AddScoped<IProductDescriptionService ,ProductDescriptionService>();
            collection.AddScoped<IReturnProductService, ReturnProductService>();
            collection.AddScoped<ITraderService, TraderService>();
            collection.AddTransient<CompaniesSearchViewModel>();
            collection.AddTransient<CompanyObjectsSearchViewModel>();
            collection.AddTransient<CompaniesDataManager>();
            collection.AddTransient<ListViewInputViewModel>();
            collection.AddTransient<ReturnProtocolController>();
            collection.AddTransient<InitialInformationViewModel>();
            collection.AddTransient<CompaniesDataViewModel>();
            collection.AddTransient<AddNewCompanyViewModel>();
            collection.AddTransient<CompanyCommandsViewModel>();
            collection.AddTransient<CompanyObjectCommandsViewModel>();
            collection.AddTransient<TradersListViewModel>();
            collection.AddTransient<TraderCommandsViewModel>();
            collection.AddSingleton<DayReportInputOptionsViewModelComponent, DayReportInputOptionsViewModelComponent>();
            collection.AddSingleton<MySqlDbReadProvider>();
            collection.AddSingleton<UserController>();
        }
    }
}
