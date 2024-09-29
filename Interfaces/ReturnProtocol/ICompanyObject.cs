using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.ReturnProtocol
{
    public interface ICompanyObject
    {
        string Name { get; }
        string Adrress { get; }
        bool BankPay { get; }
        public string Trader { get; }
    }
}
