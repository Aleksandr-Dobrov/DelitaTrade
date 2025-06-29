using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels
{
    public class ReturnProtocolInputModel
    {
        public DateTime? ReturnDate { get; set; }

        [Required]
        public string PayMethod { get; set; } = null!;

        [Required]
        public string CompanyObjectName { get; set; } = null!;

        public int CompanyObjectId { get; set; }

        public int TraderId { get; set; }

        public IEnumerable<TraderViewModel> Traders { get; set; } = new List<TraderViewModel>();

        public IEnumerable<string> PayMethods { get; set; } = new List<string>();
    }
}
