namespace DelitaTrade.Core.ViewModels
{
    public class SimpleReturnProtocolViewModel
    {
        public int Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public required string PayMethod { get; set; }
        public required string CompanyObjectName { get; set; }
        public required string TraderName { get; set; }
        public required string DriverName { get; set; }
    }
}
