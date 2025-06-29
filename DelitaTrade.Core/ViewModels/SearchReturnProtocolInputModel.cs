namespace DelitaTrade.Core.ViewModels
{
    public class SearchReturnProtocolInputModel
    {
        public string? TraderName { get; set; }

        public string? CompanyObjectName { get; set; }

        public string? CompanyObjectId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public IEnumerable<TraderViewModel> Traders { get; set; } = new List<TraderViewModel>();

        public IEnumerable<SimpleReturnProtocolViewModel> ReturnProtocols { get; set; } = new List<SimpleReturnProtocolViewModel>();
    }
}
