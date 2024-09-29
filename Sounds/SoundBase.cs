using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Interfaces.Configuration;

namespace DelitaTrade.Sounds
{
    public abstract class SoundBase : IConfigurable
    {        
        public SoundBase(SoundBaseConfiguration configuration)
        {           
        }

        public abstract SoundEfect Name {  get; }
        public abstract bool IsOn { get; }
        public abstract string Source { get; }

        public abstract void Configurate(IConfigurator configurator);

        public abstract void ConfigurationSave();                
    }
}
