namespace DelitaTrade.Core.ViewModels
{
    public class EditableReturnProtocolViewModel
    {
        public int Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public required string PayMethod { get; set; }
        public required CompanyObjectViewModel CompanyObject { get; set; }
        public required TraderViewModel Trader { get; set; }
        public string? ApproverName { get; set; }
    }
}
