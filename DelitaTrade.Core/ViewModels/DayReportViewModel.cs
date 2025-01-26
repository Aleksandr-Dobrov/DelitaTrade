using DelitaTrade.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class DayReportViewModel
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalNotPay { get; set; }
        public decimal TotalOldInvoice { get; set; }
        public decimal TotalWeight { get; set; }
        public required Dictionary<decimal, int> Banknotes { get; set; } = new();
        public decimal TotalCash { get; set; }
        public required UserViewModel User { get; set; }
        public VehicleViewModel? Vehicle { get; set; }
        public List<InvoiceViewModel> Invoices { get; set; } = new List<InvoiceViewModel>();
    }
}
