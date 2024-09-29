using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Interfaces.Configuration
{
    public interface IConfigurable
    {
        void Configurate(IConfigurator configurator);
    }
}
