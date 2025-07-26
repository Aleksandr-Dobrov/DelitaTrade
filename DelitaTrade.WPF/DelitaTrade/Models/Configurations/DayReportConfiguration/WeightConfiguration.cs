using System.Configuration;

namespace DelitaTrade.Models.Configurations.DayReportConfiguration
{
    public class WeightConfiguration : ConfigurationSection
    {
        [ConfigurationProperty(OptionsConstant.WeightIsOn, DefaultValue = false)]
        public bool IsEnabled
        {
            get { return (bool)this[OptionsConstant.WeightIsOn]; }
            set { this[OptionsConstant.WeightIsOn] = value; }
        }
    }
}
