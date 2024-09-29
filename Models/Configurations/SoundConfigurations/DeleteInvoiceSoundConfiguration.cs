using System.Configuration;

namespace DelitaTrade.Models.Configurations.SoundConfigurations
{
    public class DeleteInvoiceSoundConfiguration : SoundBaseConfiguration
    {
        [ConfigurationProperty(SoundConstants.RemoveInvoiceEfectName, DefaultValue = SoundEfect.DeleteInvoice)]
        public override SoundEfect EfectName        
        {
            get { return (SoundEfect)this[SoundConstants.RemoveInvoiceEfectName]; }
        }
        [ConfigurationProperty(SoundConstants.RemoveInvoiceName, DefaultValue = SoundConstants.RemoveInvoiceDefaultValue)]
        public override bool IsOnValueSound 
        {
            get { return (bool)this[SoundConstants.RemoveInvoiceName]; }
            set { this[SoundConstants.RemoveInvoiceName] = value; }
        }

        [ConfigurationProperty(SoundConstants.RemoveInvoiceSourceName, DefaultValue = SoundConstants.RemoveInvoiceDefaultSource)]
        public override string Source 
        {
            get { return (string)this[SoundConstants.RemoveInvoiceSourceName]; }
            set { this[SoundConstants.RemoveInvoiceSourceName] = value; }
        }

        [ConfigurationProperty(SoundConstants.RemoveInvoiceDefaulSourceName, DefaultValue = SoundConstants.RemoveInvoiceDefaultSource)]
        public override string DefaultSource => (string)this[SoundConstants.RemoveInvoiceDefaulSourceName];
    }
}
