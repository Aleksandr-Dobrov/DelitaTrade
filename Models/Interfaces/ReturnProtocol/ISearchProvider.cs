using DelitaTrade.Models.ReturnProtocol;

namespace DelitaTrade.Models.Interfaces.ReturnProtocol
{
    public interface ISearchProvider
    {
        string[] SearchId(SearchProtocolProvider searchArg);
        string[] MultipleSearchId(params SearchProtocolProvider[] parametr);
    }
}
