namespace DelitaTrade.Core.Models.ImportModels
{
    public class DayReportJsonImportModel
    {
        public string? TotalAmount { get; set; }
        public string? TotalCash { get; set; }
        public List<InvoiceJsonImportModel> Invoices { get; set; } = new List<InvoiceJsonImportModel>();
    }
}
