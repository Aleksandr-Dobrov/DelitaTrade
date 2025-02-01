using DelitaTrade.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DiContainerExtension
    {
        public static T GetService<T>(this IServiceScope scope) where T : class
        {
            return scope.ServiceProvider.GetService<T>()
                ?? throw new ArgumentNullException(ExceptionMessages.ServiceNotAvailable(nameof(T)));
        }
    }
}
