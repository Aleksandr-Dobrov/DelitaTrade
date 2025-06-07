using DelitaTrade.Areas.DayReportAreas;
using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.Interfaces.Sound;
using DelitaTrade.Models.SoundPlayers;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using DelitaTrade.ViewModels.Interfaces;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using DelitaTrade.WPFCore.Contracts;
using DelitaTrade.WPFCore.Services;
using DelitaTrade.WpfViewModels;
using System.Configuration;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.Stores;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIServiceCollectionExtension
    {        
        public static IServiceCollection AddWpfUserServiceAndUserUi(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>()
                .AddSingleton<UserController>()
                .AddTransient<LoginViewModel>()
                .AddTransient<UserLoginController>()
                .AddTransient<LoginRememberController>();

            return services;
        }

        public static IServiceCollection AddWpfApplicationViewModelsAndControllers(this IServiceCollection services)
        {
            services.AddTransient<CompaniesSearchViewModel>()
            .AddTransient<CompanyObjectsSearchViewModel>()
            .AddTransient<CompaniesDataManager>()
            .AddTransient<ListViewInputViewModel>()
            .AddTransient<ReturnProtocolController>()
            .AddTransient<InitialInformationViewModel>()
            .AddTransient<AddNewCompanyViewModel>()
            .AddTransient<CompanyCommandsViewModel>()
            .AddTransient<CompanyObjectCommandsViewModel>()
            .AddTransient<TradersListViewModel>()
            .AddTransient<TraderCommandsViewModel>()
            .AddSingleton<DayReportInputOptionsViewModelComponent, DayReportInputOptionsViewModelComponent>()
            .AddTransient<WpfCompanyViewModel>()
            .AddTransient<WpfCompanyObjectViewModel>()
            .AddTransient<LabeledStringTextBoxViewModel>()
            .AddTransient<LabeledCompanyTypeTextBoxViewModel>()
            .AddTransient<LabeledInvoiceNumberViewModel>()
            .AddTransient<LabeledPayMethodSelectableBoxViewModel>()
            .AddTransient<LabeledCurrencyViewModel>()
            .AddTransient<InvoiceInputViewModel>()
            .AddTransient<InvoiceCompaniesInputViewModel>()
            .AddTransient<InvoiceCurrencyInputViewModel>()
            .AddTransient<LabeledWeightTextBoxViewModel>()
            .AddTransient<InvoiceInputCommandsViewModel>()
            .AddTransient<DayReportArea>()
            .AddTransient<DayReportLoaderViewModel>()
            .AddTransient<DayReportListIdViewModel>()
            .AddTransient<IDayReportCrudController, DayReportCrudController>()
            .AddTransient<DayReportTotalsViewModel>()
            .AddTransient<DayReportCommandsViewModel>()
            .AddTransient<InvoicesListController>()
            .AddTransient<PayDeskViewModel>()
            .AddTransient<VehiclesListViewModel>()
            .AddTransient<AdvanceViewModel>()
            .AddTransient<DayReportExportCommandViewModel>()
            .AddTransient<ReturnProtocolExportCommandsViewModel>()
            .AddTransient<InternetProvider>()
            .AddTransient<InternetProviderViewModel>()
            .AddTransient<ImportProductsController>()
            .AddTransient<MainViewModel>()
            .AddTransient<DayReportExporter>()
            .AddTransient<ReturnProtocolExporter>()
            .AddSingleton<SoundStore>()
            .AddTransient<ProductSearchController>()
            .AddSingleton<ISoundPlayable, DefaultSoundPlayer>()
            .AddSingleton<DelitaSoundService>()
            .AddTransient<OptionsViewModel>()
            .AddSingleton<SoundOptionsViewModel>()
            .AddTransient<DateIntervalViewModel>()
            .AddSingleton<DescriptionCategoryManagerController>()
            .AddTransient<DescriptionCategoryController>()
            .AddTransient<ReturnProtocolBuildersController>();

            return services;
        }

        public static IServiceCollection AddWpfApplicationExporterServices(this IServiceCollection services)
        {

            services.AddTransient<IReturnProtocolBuildersStore, ReturnProtocolBuildersStore>()
                .AddSingleton<IReturnProtocolBuilderService, ReturnProtocolBuilderService>()
                .AddTransient<ExcelReturnProtocolBuilder>()
                .AddTransient<ExcelCrisisReturnProtocolBuilder>()
                .AddTransient<ExcelStupidReturnProtocolBuilder>()
                .AddTransient<ExcelStupidCardReturnProtocolBuilder>();

            return services;
        }
    }
}
