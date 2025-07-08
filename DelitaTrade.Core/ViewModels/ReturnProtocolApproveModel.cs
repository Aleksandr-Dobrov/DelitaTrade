using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels
{
    public class ReturnProtocolApproveModel
    {
        public int Id { get; set; }

        public DateTime? LastChange { get; set; }

        public UserViewModel? Approver { get; set; }

        public IEnumerable<ReturnedProductApproveModel> ReturnedProducts { get; set; } = new List<ReturnedProductApproveModel>();
    }
}
