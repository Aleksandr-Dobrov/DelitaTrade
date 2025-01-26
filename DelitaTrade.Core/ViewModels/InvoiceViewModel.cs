using DelitaTrade.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public int IdInDayReport { get; set; }
        public required string Number { get; set; }
        public required CompanyViewModel Company { get; set; }
        public required CompanyObjectViewModel CompanyObject { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalWeight { get; set; }
        public PayMethod PayMethod { get; set; }
        public bool IsPaid { get; set; }
    }
}
