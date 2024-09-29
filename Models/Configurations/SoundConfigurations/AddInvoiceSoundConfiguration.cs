using System.Configuration;

namespace DelitaTrade.Models.Configurations.SoundConfigurations
{
    public class AddInvoiceSoundConfiguration : SoundBaseConfiguration
    {
        [ConfigurationProperty(SoundConstants.AddInvoiseEfectName, DefaultValue = SoundEfect.AddInvoice)]
        public override SoundEfect EfectName
        {
            get { return (SoundEfect)this[SoundConstants.AddInvoiseEfectName]; }
        }

        [ConfigurationProperty(SoundConstants.AddInvoiceName, DefaultValue = SoundConstants.AddInvoiceDefaultValue)]
        public override bool IsOnValueSound
        {
            get { return (bool)this[SoundConstants.AddInvoiceName]; }
            set { this[SoundConstants.AddInvoiceName] = value; }
        }

        [ConfigurationProperty(SoundConstants.AddInvoiceSourseName, DefaultValue = SoundConstants.AddInvoiceDefaultSource)]
        public override string Source
        {
            get { return (string)this[SoundConstants.AddInvoiceSourseName]; }
            set { this[SoundConstants.AddInvoiceSourseName] = value; }
        }

        [ConfigurationProperty(SoundConstants.AddInvoicDefaultSourseName, DefaultValue = SoundConstants.AddInvoiceDefaultSource)]
        public override string DefaultSource => (string)this[SoundConstants.AddInvoicDefaultSourseName];
    }
}
