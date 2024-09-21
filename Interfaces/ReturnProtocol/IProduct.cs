using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.ReturnProtocol
{    
    public interface IProduct
    {      
        public string ItemName { get; }
        public string Unit { get; }        
    }
}
