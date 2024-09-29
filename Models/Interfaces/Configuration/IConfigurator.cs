using DelitaTrade.Models.DataProviders;

namespace DelitaTrade.Models.Interfaces.Configuration
{
    public interface IConfigurator
    {
        ConfigurationProvider Provider { get; }        
    }
}
