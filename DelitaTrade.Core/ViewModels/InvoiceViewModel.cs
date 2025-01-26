using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Interfaces;
using DelitaTrade.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class InvoiceViewModel : IExceptionName
    {
        public int Id { get; set; }
        public int IdInDayReport { get; set; }
        public required string Number { get; set; }
        public required CompanyViewModel Company { get; set; }
        public required CompanyObjectViewModel CompanyObject { get; set; }
        public DayReportViewModel? DayReport { get; set; }
        public decimal Amount { get; set; }
        public decimal Income { get; set; }
        public decimal Weight { get; set; }
        public PayMethod PayMethod { get; set; }
        public bool IsPaid { get; set; }

        public string Name => nameof(InvoiceInDayReport);
    }
}
