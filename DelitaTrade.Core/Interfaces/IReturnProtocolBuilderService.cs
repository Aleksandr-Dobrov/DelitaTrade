using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Interfaces
{
    public interface IReturnProtocolBuilderService
    {
        IEnumerable<string> GetAll();
        void SelectBuilder(string name);
        IReturnProtocolBuilder GetSelectedBuilder();
    }
}
