using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Services;
using DelitaTrade.WPFCore.Contracts;
using DelitaTrade.WPFCore.Services;
using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.ViewModels;
using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using DelitaTrade.WpfViewModels;
using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Areas.DayReportAreas;
using System.Configuration;
using DelitaTrade.Stores;
using DelitaTrade.Models.Interfaces.Sound;
using DelitaTrade.Models.SoundPlayers;
using DelitaTrade.Services;
using DelitaTrade.ViewModels.Interfaces;
using DelitaTrade.Models.DataProviders;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Core.Stores;

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
            collection.AddScoped<IVehicleService, VehicleService>();
            collection.AddScoped<IInvoiceInDayReportService, InvoiceInDayReportService>();
            collection.AddScoped<IDayReportService, DayReportService>();
            collection.AddScoped<IInvoicePaymentService, InvoicePaymentService>();
            collection.AddScoped<IBanknotesService, BanknotesService>();
            collection.AddScoped<IDescriptionCategoryService, DescriptionCategoryService>();
            collection.AddTransient<CompaniesSearchViewModel>();
            collection.AddTransient<CompanyObjectsSearchViewModel>();
            collection.AddTransient<CompaniesDataManager>();
            collection.AddTransient<ListViewInputViewModel>();
            collection.AddTransient<ReturnProtocolController>();
            collection.AddTransient<InitialInformationViewModel>();
            collection.AddTransient<AddNewCompanyViewModel>();
            collection.AddTransient<LoginViewModel>();
            collection.AddTransient<UserLoginController>();
            collection.AddTransient<LoginRememberController>();
            collection.AddTransient<CompanyCommandsViewModel>();
            collection.AddTransient<CompanyObjectCommandsViewModel>();
            collection.AddTransient<TradersListViewModel>();
            collection.AddTransient<TraderCommandsViewModel>();
            collection.AddSingleton<DayReportInputOptionsViewModelComponent, DayReportInputOptionsViewModelComponent>();
            collection.AddTransient<WpfCompanyViewModel>();
            collection.AddTransient<WpfCompanyObjectViewModel>();
            collection.AddTransient<LabeledStringTextBoxViewModel>();
            collection.AddTransient<LabeledCompanyTypeTextBoxViewModel>();
            collection.AddTransient<LabeledInvoiceNumberViewModel>();
            collection.AddTransient<LabeledPayMethodSelectableBoxViewModel>();
            collection.AddTransient<LabeledCurrencyViewModel>();
            collection.AddTransient<InvoiceInputViewModel>();
            collection.AddTransient<InvoiceCompaniesInputViewModel>();
            collection.AddTransient<InvoiceCurrencyInputViewModel>();
            collection.AddTransient<LabeledWeightTextBoxViewModel>();
            collection.AddTransient<InvoiceInputCommandsViewModel>();
            collection.AddTransient<DayReportArea>();
            collection.AddTransient<DayReportLoaderViewModel>();
            collection.AddTransient<DayReportListIdViewModel>();
            collection.AddTransient<IDayReportCrudController, DayReportCrudController>();
            collection.AddTransient<DayReportTotalsViewModel>();
            collection.AddTransient<DayReportCommandsViewModel>();
            collection.AddTransient<InvoicesListController>();
            collection.AddTransient<PayDeskViewModel>();
            collection.AddTransient<VehiclesListViewModel>();
            collection.AddTransient<AdvanceViewModel>();
            collection.AddTransient<DayReportExportCommandViewModel>();
            collection.AddTransient<ReturnProtocolExportCommandsViewModel>();
            collection.AddTransient<InternetProvider>();
            collection.AddTransient<InternetProviderViewModel>();
            collection.AddTransient<ImportProductsController>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<DayReportExporter>();
            collection.AddTransient<ReturnProtocolExporter>();
            collection.AddSingleton<UserController>();
            collection.AddSingleton<SoundStore>();
            collection.AddTransient<ProductSearchController>();
            collection.AddSingleton<ISoundPlayable, DefaultSoundPlayer>();
            collection.AddSingleton<DelitaSoundService>();
            collection.AddTransient<OptionsViewModel>();
            collection.AddSingleton<SoundOptionsViewModel>();
            collection.AddTransient<DateIntervalViewModel>();
            collection.AddSingleton<DescriptionCategoryManagerController>();
            collection.AddTransient<DescriptionCategoryController>();
            collection.AddTransient<ReturnProtocolBuildersController>();
            collection.AddTransient<IReturnProtocolBuildersStore, ReturnProtocolBuildersStore>();
            collection.AddSingleton<IReturnProtocolBuilderService, ReturnProtocolBuilderService>();
            collection.AddTransient<ExcelReturnProtocolBuilder>();
            collection.AddTransient<ExcelCrisisReturnProtocolBuilder>();
            collection.AddTransient<ExcelStupidReturnProtocolBuilder>();
            collection.AddTransient<ExcelStupidCardReturnProtocolBuilder>();
            collection.AddSingleton(System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
        }
    }
}
