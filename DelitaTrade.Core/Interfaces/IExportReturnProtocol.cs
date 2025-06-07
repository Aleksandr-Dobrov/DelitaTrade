namespace DelitaTrade.Core.Interfaces
{
    public interface IExportReturnProtocol
    {
        string Id { get; }
        string ReturnDate { get; }
        string CompanyObject { get; }
        string Address { get; }
        string TraderName { get; }
        string PayMethod { get; }
        string UserName { get; }
        IEnumerable<IExportedReturnedProduct> Products { get; }
    }
}
