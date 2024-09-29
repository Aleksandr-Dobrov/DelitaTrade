using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.ReturnProtocol
{
    public interface IReturnProtocolData
    {
        public string ID { get; }
        public string CompanyFullName { get; }
        public string ObjectName { get; }
        public string Trader { get; }
        public string DateString { get; }
    }
}
