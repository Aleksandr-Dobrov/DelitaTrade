using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ConfigurationModels
{
    public class ReturnProtocolBuilderConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("builder", DefaultValue = "ExcelReturnProtocolBuilder")]
        public string Builder
        {
            get => (string)this["builder"];
            set => this["builder"] = value;
        }
    }
}
