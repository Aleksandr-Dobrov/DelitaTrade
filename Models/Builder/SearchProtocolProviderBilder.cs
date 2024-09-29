using DelitaTrade.Models.Interfaces.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocol;

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
