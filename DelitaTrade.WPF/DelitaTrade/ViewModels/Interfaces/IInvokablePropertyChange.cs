using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface IInvokablePropertyChange
    {
        void InvokePropertyChange(string propertyName);
    }
}
