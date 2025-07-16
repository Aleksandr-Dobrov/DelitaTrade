using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class DayReportInputModel
    {
        public DateTime? ReportedDate { get; set; }

        [Range(1, int.MaxValue)]
        public int VehicleId { get; set; }

        [Required]
        public string UserName { get; set; } = null!;

        public IEnumerable<VehicleViewModel> Vehicles { get; set; } = new List<VehicleViewModel>();
        public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}
