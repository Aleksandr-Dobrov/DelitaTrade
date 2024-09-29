using DelitaTrade.Interfaces.ReturnProtocol;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.ReturnProtocol
{
    [DataContract]
    public class ReturnProtocolDataBase : ISearchProvider
    {
        public const string savePath = "../../../ReturnProtocol/ReturnProtocolsDataBase/ReturnProtocolDataBase.xml";
        [DataMember]
        private HashSet<string> _id;
        [DataMember]
        private Dictionary<string, HashSet<string>> _company;
        [DataMember]               
        private Dictionary<string, HashSet<string>> _companyObject;
        [DataMember]               
        private Dictionary<string, HashSet<string>> _date;
        [DataMember]               
        private Dictionary<string, HashSet<string>> _trader;

        public ReturnProtocolDataBase()
        {
            _id = new HashSet<string>();
            _company = new Dictionary<string, HashSet<string>>();
            _companyObject = new Dictionary<string, HashSet<string>>();
            _date = new Dictionary<string, HashSet<string>>();
            _trader = new Dictionary<string, HashSet<string>>();
            DataBaseChange += () => { };
        }

        public event Action DataBaseChange;
        public int Count => _id.Count;

        public void AddReturnProtocol(IReturnProtocolData returnProtocol)
        {
            if (_id.Contains(returnProtocol.ID) == false)
            {
                _id.Add(returnProtocol.ID);
            }
            else
            {
                throw new ArgumentException("Return Protocol is already exists");
            }

            string[] data = [returnProtocol.CompanyFullName, returnProtocol.ObjectName, returnProtocol.DateString, returnProtocol.Trader];
            Dictionary<string, HashSet<string>>[] dataToSafe =
            {
                _company,
                _companyObject,
                _date,
                _trader,
            };

            for (int i = 0; i < dataToSafe.Length; i++)
            {
                AddId(ref dataToSafe[i], data[i],returnProtocol.ID);
            }
            DataBaseChange();
        }

        public void UpdateReturnProtocol(IReturnProtocolData oldReturnProtocol, IReturnProtocolData newReturnProtocol)
        {
            if (oldReturnProtocol.ID != newReturnProtocol.ID)
            {
                throw new InvalidOperationException($"The return protocols must have equals ID{Environment.NewLine}changed ID{oldReturnProtocol.ID} != new data ID{newReturnProtocol.ID}");
            }
            if (_id.Contains(oldReturnProtocol.ID) == false)
            {
                throw new ArgumentException("Return Protocol is not exists");
            }

            IReturnProtocolData[] returnProtocolDatas = [oldReturnProtocol, newReturnProtocol];
            Dictionary<string, HashSet<string>>[] dataToSafe =
            {
                _company,
                _companyObject,
                _date,
                _trader,
            };

            for (int i = 0; i < returnProtocolDatas.Length; i++)
            { 
                string[] data = [returnProtocolDatas[i].CompanyFullName,
                                 returnProtocolDatas[i].ObjectName, 
                                 returnProtocolDatas[i].DateString,
                                 returnProtocolDatas[i].Trader];
                if (i == 0)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        RemoveId(ref dataToSafe[j], data[j], oldReturnProtocol.ID);
                    }

                }
                else 
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        AddId(ref dataToSafe[j], data[j], oldReturnProtocol.ID);
                    }
                }
            }
            DataBaseChange();
        }

        public void DeleteReturnProtocol(IReturnProtocolData returnProtocol)
        {
            if (_id.Contains(returnProtocol.ID) == false)
            {
                throw new ArgumentException("Return Protocol is not exists");
            }
            
            _id.Remove(returnProtocol.ID);

            string[] data = [returnProtocol.CompanyFullName, returnProtocol.ObjectName, returnProtocol.DateString, returnProtocol.Trader];
            Dictionary<string, HashSet<string>>[] dataToSafe =
            {
                _company,
                _companyObject,
                _date,
                _trader,
            };

            for (int i = 0; i < dataToSafe.Length; i++)
            {
                RemoveId(ref dataToSafe[i], data[i], returnProtocol.ID);
            }
            DataBaseChange();
        }

        public string[] SearchId(SearchProtocolProvider searchArg)
        { 
            return GetIdFromFunc(searchArg);
        }

        public string[] MultipleSearchId(params SearchProtocolProvider[] parametr)
        {
            string[][] id = new string[parametr.Length][];

            for (int i = 0; i < parametr.Length; i++)
            {
                id[i] = GetIdFromFunc(parametr[i]);
            }
            
            return GetUniqueId(id);
        }

        private string[] SearchIdByCompany(ISearchParametr company)
        {
            if (_company.ContainsKey(company.SearchParametr))
            { 
                return [.. _company[company.SearchParametr]];
            }
            return [];
        }

        private string[] SearchIdByCompanyObject(ISearchParametr companyObject)
        {
            if (_companyObject.ContainsKey(companyObject.SearchParametr))
            { 
                return [.. _companyObject[companyObject.SearchParametr]];
            }
            return [];
        }

        private string[] SearchIdByDate(ISearchParametr date)
        {
            if (_date.ContainsKey(date.SearchParametr))
            { 
                return [.. _date[date.SearchParametr]];
            }
            return [];
        }

        private string[] SearchIdByTrader(ISearchParametr trader)
        {
            if (_trader.ContainsKey(trader.SearchParametr))
            { 
                return [.. _trader[trader.SearchParametr]];
            }
            return [];
        }

        private string[] GetIdFromFunc(SearchProtocolProvider search) =>
        
            search.Method switch
            {
                SearchMethod.CompanyName => SearchIdByCompany(search.Parametr),
                SearchMethod.ObjectName => SearchIdByCompanyObject(search.Parametr),
                SearchMethod.Date => SearchIdByDate(search.Parametr),
                SearchMethod.Trader => SearchIdByTrader(search.Parametr),
                _ => []
            };
        

        private string[] GetUniqueId(params string[][] id)
        {
            Dictionary<string,int> ids = new Dictionary<string,int>();

            for (int i = 0; i < id.Length; i++)
            {
                for (int j = 0; j < id[i].Length; j++)
                {
                    if (ids.ContainsKey(id[i][j]) == false)
                    {
                        ids.Add(id[i][j], 1);
                    }
                    else
                    {
                        ids[id[i][j]]++;
                    }
                }
            }
            return [.. ids.Where(i => i.Value == id.Length).ToDictionary().Keys];
        }

        private void AddId(ref Dictionary<string, HashSet<string>> data, string parametr, string id)
        {
            if (data.ContainsKey(parametr))
            {
                data[parametr].Add(id);
            }
            else
            {
                data[parametr] = [id];
            }
        }

        private void RemoveId(ref Dictionary<string, HashSet<string>> data, string parametr, string id)
        {
            data[parametr].Remove(id);
        }
    }
}
