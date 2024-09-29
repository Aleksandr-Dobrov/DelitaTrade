using DelitaTrade.Models.Configurations;

namespace DelitaTrade.Models.DataProviders
{
    public class SoudFXConfigurationProvider : ConfigurationProvider
    {
        public SoudFXConfigurationProvider(SoundEfect name, bool isOn, string sourceValue) : base(name, isOn, sourceValue)
        {
        }
    }
}
