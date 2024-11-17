using DelitaTrade.Models.Interfaces.DataBase;

namespace DelitaTrade.Models.DataBases.DayReportDataBase
{
    public class DBInvoiceInDayReport : IDBData
    {
        private int _id;
        private string _userName;
        private string _dayReportId;
        private string _payMethod;
        private string _invoiceId;
        private decimal _income;

        public DBInvoiceInDayReport(string userName, string dayReportId, int id, string invoiceId, string payMethod, decimal totalIncome)
        {
            _userName = userName;
            _dayReportId = dayReportId;
            _id = id;
            _invoiceId = invoiceId;
            _payMethod = payMethod;
            _income = totalIncome;
        }

        public string UserName => _userName;
        public string DayReportId => _dayReportId;
        public int Id => _id;
        public string InvoiceId => _invoiceId;
        public string PayMethod => _payMethod;
        public decimal Income => _income;

        public string Parameters => "user_name-=-day_report_id-=-new_id-=-new_invoice_id-=-new_pay_method-=-invoice_Income";

        public string Data => $"{UserName}-=-{DayReportId}-=-{Id}-=-{InvoiceId}-=-{PayMethod}-=-{Income}";

        public string Procedure => "add_invioce_toDayReport";

        public int NumberOfAdditionalParameters => 0;
    }
}
