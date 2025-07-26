using System.Configuration;

namespace DelitaTrade.Models.Configurations.SoundConfigurations
{
    public class CashSoundConfiguration : SoundBaseConfiguration
    {
        [ConfigurationProperty(SoundConstants.CashEfectName, DefaultValue = SoundEfect.Cash)]
        public override SoundEfect EfectName
        {
            get { return (SoundEfect)this[SoundConstants.CashEfectName]; }
        }

        [ConfigurationProperty(SoundConstants.CashIsOnValueName, DefaultValue = SoundConstants.CashDefaultIsOnValue)]
        public override bool IsOnValueSound
        {
            get { return (bool)this[SoundConstants.CashIsOnValueName]; }
            set { this[SoundConstants.CashIsOnValueName] = value; }
        }

        [ConfigurationProperty(SoundConstants.CashSourseName, DefaultValue = SoundConstants.CashSourceDefaultValue)]
        public override string Source
        {
            get { return (string)this[SoundConstants.CashSourseName]; }
            set { this[SoundConstants.CashSourseName] = value; }
        }

        [ConfigurationProperty(SoundConstants.CashDefaultSourseName, DefaultValue = SoundConstants.CashSourceDefaultValue)]
        public override string DefaultSource => (string)this[SoundConstants.CashDefaultSourseName];
    }
}
