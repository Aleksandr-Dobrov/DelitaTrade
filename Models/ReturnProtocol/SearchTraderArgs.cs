using DelitaTrade.Interfaces.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
