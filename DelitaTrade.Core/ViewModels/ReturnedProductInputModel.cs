using System.ComponentModel.DataAnnotations;


namespace DelitaTrade.Core.ViewModels
{    
    public class ReturnedProductInputModel
    {
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string Unit { get; set; } = null!;

        [Range(0.001, double.MaxValue)]
        public double Quantity { get; set; }

        public DateTime BestBefore { get; set; } = DateTime.Now;

        [Required]
        public string Batch { get; set; } = null!;

        public int? DescriptionId { get; set; }
        public string? Description { get; set; }

        public int ReturnProtocolId { get; set; }

        public int DescriptionCategoryId { get; set; }

        public IEnumerable<DescriptionCategoryViewModel> DescriptionCategories { get; set; } = new List<DescriptionCategoryViewModel>();

    }
}
