using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Interfaces.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.DataProviders
{
    public abstract class ConfigurationProvider : IConfigurator
    {
        public SoundEfect Name { get; }

        public virtual bool IsOn { get; }

        public virtual string SourceValue { get; }

        public ConfigurationProvider Provider => this;

        protected ConfigurationProvider(SoundEfect name, bool isOn, string sourceValue)
        {
            Name = name;
            IsOn = isOn;
            SourceValue = sourceValue;
        }
                
    }
}
