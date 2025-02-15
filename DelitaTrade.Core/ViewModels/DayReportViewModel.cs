using DelitaTrade.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class DayReportViewModel : DayReportHeaderViewModel
    {
        public DateTime TransmissionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalNotPay { get; set; }
        public decimal TotalOldInvoice { get; set; }
        public decimal TotalWeight { get; set; }
        public Dictionary<decimal, int> Banknotes { get; set; } = new Dictionary<decimal, int>
            {
                { 0.01m, 0 },
                { 0.02m, 0 },
                { 0.05m, 0 },
                { 0.1m, 0 },
                { 0.2m, 0 },
                { 0.5m, 0 },
                { 1.0m, 0 },
                { 2.0m, 0 },
                { 5.0m, 0 },
                { 10.0m, 0 },
                { 20.0m, 0 },
                { 50.0m, 0 },
                { 100.0m, 0 },
            };
        public decimal TotalCash { get; set; }
        public required UserViewModel User { get; set; }
        public VehicleViewModel? Vehicle { get; set; }
        public List<InvoiceViewModel> Invoices { get; set; } = new List<InvoiceViewModel>();
    }
}
