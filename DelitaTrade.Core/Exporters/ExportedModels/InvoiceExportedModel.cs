using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExportedModels
{
    public class InvoiceExportedModel(bool isPaid, string number, string companyFullName, string objectName, decimal amount, decimal income, PayMethod payMethod) : IExportedInvoice
    {
        public bool IsPaid { get; private set; } = isPaid;

        public string Number { get; private set; } = number;

        public string CompanyFullName { get; private set; } = companyFullName;

        public string ObjectName { get; private set; } = objectName;

        public decimal Amount { get; private set; } = amount;

        public decimal Income { get; private set; } = income;

        public PayMethod PayMethod { get; private set; } = payMethod;
    }
}
