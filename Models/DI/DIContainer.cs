using DBDelitaTrade.Infrastructure.Data;
using DelitaReturnProtocolProvider.Services;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using DelitaTrade.Models.MySqlDataBase;
using DelitaTrade.Models.ReturnProtocolSQL;
using DelitaTrade.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DelitaTrade.Models.DI
{
    public class DIContainer
    {
        public static IServiceProvider BuildServiceProvider() 
        {
            ServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<DayReportInputOptionsViewModelComponent, DayReportInputOptionsViewModelComponent>();
            collection.AddSingleton<MySqlDbReadProvider>();
            collection.AddDbContext<DelitaDbContext>(ServiceLifetime.Transient);
            collection.AddTransient<ProductService>();
            collection.AddTransient<ProductDescriptionService>();
            collection.AddTransient<ReturnProtocolService>();
            collection.AddTransient<ReturnProductService>();
            collection.AddSingleton<UserService>();

            return collection.BuildServiceProvider();
        }
    }
}
