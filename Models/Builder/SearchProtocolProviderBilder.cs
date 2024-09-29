using DelitaTrade.Interfaces.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Builder
{
    public class SearchProtocolProviderBilder
    {
        public SearchProtocolProvider GetSearchProvider(ISearchParametr searchParametr)
        {
            return new SearchProtocolProvider(searchParametr.GetSearchMethod(), searchParametr);
        }
    }
}
