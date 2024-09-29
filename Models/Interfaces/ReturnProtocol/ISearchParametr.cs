using DelitaTrade.Models.ReturnProtocol;

namespace DelitaTrade.Models.Interfaces.ReturnProtocol
{
    public interface ISearchParametr
    {
        string SearchParametr { get; }

        SearchMethod GetSearchMethod();
    }
}
