using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface ICompanyObjectData
    {
        string ObjectName { get; }
        bool BankPay { get; }
        string Town { get; }
        string Street { get; }
        string Number { get; }
        string GpsCoordinates { get; }
        string Description { get; }
    }
}
