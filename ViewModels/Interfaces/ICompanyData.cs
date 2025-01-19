using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface ICompanyData
    {
        string CompanyName { get; }
        string CompanyType { get; }
        string Bulstad {  get; }
    }
}
