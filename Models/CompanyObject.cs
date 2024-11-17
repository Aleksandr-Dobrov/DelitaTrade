using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.Interfaces.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocol;

namespace DelitaTrade.Models
{
    public class CompanyObject : ICompanyObject, ISearchParametr, IDBData
    {
        private const int _numberOfReferences = 1;
        private string _name;
        private string _adrress;
        private bool _bankPay;
        private string _companyName;
        private string _trader;

        public CompanyObject(string companyName, string name, string adrress, string trader, bool bankPay)
        {
            _name = name;
            _adrress = adrress;
            _bankPay = bankPay;
            _trader = trader;
            _companyName = companyName;
        }

        public string CompanyName => _companyName;
        public string Name => _name;
        public string Adrress => _adrress;
        public bool BankPay => _bankPay;
        public string Trader => _trader;

        public string Parameters => "company_name-=-object_name-=-object_address-=-object_trader-=-object_bank_pay";
        public string Data => $"{CompanyName}-=-{Name}-=-{Adrress}-=-{Trader}-=-{Convert.ToInt32(BankPay)}";
        public string Procedure => "add_object_full";

        public string SearchParametr => Name;

        public int NumberOfAdditionalParameters => _numberOfReferences;

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

        public override string ToString()
        {
            return Name;
        }
    }
}
