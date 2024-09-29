using DelitaTrade.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.DataProviders
{
    public class SoudFXConfigurationProvider : ConfigurationProvider
    {
        public SoudFXConfigurationProvider(SoundEfect name, bool isOn, string sourceValue) : base(name, isOn, sourceValue)
        {
        }
    }
}
