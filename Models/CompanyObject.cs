using DelitaTrade.Interfaces.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocol;
using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class CompanyObject : ICompanyObject, ISearchParametr
    {
        [DataMember]
        private string _name;
        [DataMember]
        private string _adrress;
        [DataMember]
        private bool _bankPay;
        [DataMember]
        private Company _company;
        [DataMember]
        private string _trader;

        public CompanyObject(string name, string adrress, string trader, bool bankPay)
        {
            _name = name;
            _adrress = adrress;
            _bankPay = bankPay;
            _trader = trader;
        }

        public string SearchParametr => Name;
        public string Name => _name;
        public string Adrress => _adrress;
        public bool BankPay => _bankPay;
        public string Trader => _trader;

        public SearchMethod GetSearchMethod()
        {
            return SearchMethod.ObjectName;
        }

        public void UpdateCompanyObject(CompanyObject companyObject)
        { 
            _adrress = companyObject.Adrress;
            _bankPay = companyObject.BankPay;
            _trader = companyObject.Trader;
        }
    }
}
