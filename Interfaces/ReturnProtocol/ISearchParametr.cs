using DelitaTrade.Models.ReturnProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.ReturnProtocol
{
    public interface ISearchParametr
    {
        string SearchParametr { get; }

        SearchMethod GetSearchMethod();
    }
}
