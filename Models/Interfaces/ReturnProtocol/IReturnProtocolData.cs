namespace DelitaTrade.Models.Interfaces.ReturnProtocol
{
    public interface IReturnProtocolData
    {
        public string ID { get; }
        public string CompanyFullName { get; }
        public string ObjectName { get; }
        public string Trader { get; }
        public string DateString { get; }
    }
}
