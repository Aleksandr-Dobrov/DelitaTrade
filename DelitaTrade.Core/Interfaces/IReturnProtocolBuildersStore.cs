using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Interfaces
{
    public interface IReturnProtocolBuildersStore
    {
        IEnumerable<IReturnProtocolBuilder> GetBuilders();

        IReturnProtocolBuilder? GetBuilderByName(string name);

        void AddBuilders(params IReturnProtocolBuilder[] builder);
    }
}
