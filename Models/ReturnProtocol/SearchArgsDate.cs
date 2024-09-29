using DelitaTrade.Interfaces.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class SearchArgsDate : ISearchParametr
    {
        string _arg;
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
