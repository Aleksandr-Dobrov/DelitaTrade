using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Exporters.ExportedModels
{
    public class ReturnProtocolExportedModel : IExportReturnProtocol
    {
        public ReturnProtocolExportedModel(string id,string returnDate, string companyObject, string address, string traderName, string payMethod, string userName, IEnumerable<IExportedReturnedProduct> products)
        {
            Id = id;
            ReturnDate = returnDate;
            CompanyObject = companyObject;
            Address = address;
            TraderName = traderName;
            PayMethod = payMethod;
            UserName = userName;
            Products = products;
        }

        public string Id { get; private set; }
        public string ReturnDate { get; private set; }
        public string CompanyObject { get; private set; }
        public string Address { get; private set; }
        public string TraderName { get; private set; }
        public string PayMethod { get; private set; }
        public string UserName { get; private set; }
        public IEnumerable<IExportedReturnedProduct> Products { get; private set; }
    }
}
