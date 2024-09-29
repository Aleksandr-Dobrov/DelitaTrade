using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Interfaces.Configuration;

namespace DelitaTrade.Models.DataProviders
{
    public abstract class ConfigurationProvider : IConfigurator
    {
        public ConfigurationProvider Provider => this;

        public SoundEfect Name { get; }

        public virtual bool IsOn { get; }

        public virtual string SourceValue { get; }

        protected ConfigurationProvider(SoundEfect name, bool isOn, string sourceValue)
        {
            Name = name;
            IsOn = isOn;
            SourceValue = sourceValue;
        }                
    }
}
