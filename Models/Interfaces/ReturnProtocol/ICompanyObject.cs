namespace DelitaTrade.Models.Interfaces.ReturnProtocol
{
    public interface ICompanyObject
    {
        string Name { get; }
        string Adrress { get; }
        bool BankPay { get; }
        public string Trader { get; }
    }
}
