using DelitaTrade.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface IExportedInvoice
    {
        bool IsPaid { get; }
        string Number { get; }
        string CompanyFullName { get; }
        string ObjectName { get; }
        decimal Amount { get; }
        decimal Income { get; }
        PayMethod PayMethod { get; }
    }
}
