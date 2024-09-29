using DelitaTrade.Models.Interfaces.ReturnProtocol;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class SearchArgsDate : ISearchParametr
    {
        private string _arg;

        public SearchArgsDate(DateTime date)
        {
            _arg = date.Date.ToString("dd-MM-yyyy");
        }

        public string SearchParametr => _arg;

        public SearchMethod GetSearchMethod()
        {
            return SearchMethod.Date;
        }
    }
}
