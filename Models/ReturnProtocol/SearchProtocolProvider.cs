using DelitaTrade.Interfaces.ReturnProtocol;

namespace DelitaTrade.Models.ReturnProtocol
{
    public enum SearchMethod
    {
        CompanyName,
        ObjectName,
        Trader,
        Date
    }

    public class SearchProtocolProvider
    {
        private SearchMethod _searchMethod;
        private ISearchParametr _parametr;

        public SearchProtocolProvider(SearchMethod searchMethod, ISearchParametr parametr)
        {
            _parametr = parametr;
            _searchMethod = searchMethod;
        }

        public SearchMethod Method => _searchMethod;
        public ISearchParametr Parametr => _parametr;

    }
}
