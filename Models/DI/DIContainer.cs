using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DelitaTrade.Models.DI
{
    public class DIContainer
    {
        public static IServiceProvider BuildServiceProvider() 
        {
            ServiceCollection collection = new ServiceCollection();
            collection.AddSingleton<DayReportInputOptionsViewModelComponent, DayReportInputOptionsViewModelComponent>();

            return collection.BuildServiceProvider();
        }
    }
}
