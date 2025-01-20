using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Core.Services;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Extensions
{
    public static class DiContainerExtension
    {
        private static CompanyService GetNewService(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetService<CompanyService>()
                ?? throw new ArgumentNullException(ExceptionMessages.ServiceNotAvailable(nameof(CompanyService)));
        }
    }
}
