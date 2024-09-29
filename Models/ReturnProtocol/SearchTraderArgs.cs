using DelitaTrade.Models.Interfaces.ReturnProtocol;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class SearchTraderArgs : ISearchParametr
    {
        private string _arg;

        public SearchTraderArgs(string arg)
        {
            _arg = arg;
        }
        
        public virtual string SearchParametr => _arg;

        public SearchMethod GetSearchMethod()
        {
            return SearchMethod.Trader;
        }
    }
}
