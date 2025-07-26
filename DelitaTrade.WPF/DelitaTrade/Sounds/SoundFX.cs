using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Interfaces.Configuration;
using System.IO;

namespace DelitaTrade.Sounds
{
    public class SoundFX : SoundBase
    {
        private readonly SoundBaseConfiguration _configuration;

        public SoundFX(SoundBaseConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override SoundEfect Name => _configuration.EfectName;

        public override bool IsOn => _configuration.IsOnValueSound;

        public override string Source => GetSourceOrDefault();

        public override void Configurate(IConfigurator configurator)
        {
            _configuration.IsOnValueSound = configurator.Provider.IsOn;
            _configuration.Source = configurator.Provider.SourceValue;
        }

        public override void ConfigurationSave()
        {
            _configuration.CurrentConfiguration.Save();
        }

        private string GetSourceOrDefault()
        {
            if (File.Exists(_configuration.Source))
            {
                return _configuration.Source;
            }
            else
            {
                return _configuration.DefaultSource;
            }
        }
    }
}
