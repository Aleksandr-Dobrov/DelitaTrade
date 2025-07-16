using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class DeliveryInputModel
    {
        public int DayReportId { get; set; }
        public int DeliveryAddressId { get; set; }
        [Required]
        public string DeliveryAddressName { get; set; } = null!;

        [Required]
        public string PayMethod { get; set; } = null!;

        public IList<string> PayMethods { get; set; } = new List<string>();
    }
}
