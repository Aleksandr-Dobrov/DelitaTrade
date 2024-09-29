using DelitaTrade.Models.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.ReturnProtocol
{
    public interface ISearchProvider
    {
        string[] SearchId(SearchProtocolProvider searchArg);
        string[] MultipleSearchId(params SearchProtocolProvider[] parametr);
    }
}
