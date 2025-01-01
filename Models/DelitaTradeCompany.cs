using DBDelitaTrade.Infrastructure.Data;
using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DelitaTrade.Models
{
    public class DelitaTradeCompany
    {
        private CompaniesDataBase _dataBase;
        private readonly IDBProvider _mySqlDataBase;
        private IServiceProvider _serviceProvider;

        public DelitaTradeCompany(string name, IDBProvider dataBase, IServiceProvider serviceProvider)
        {
            Name = name;
            _mySqlDataBase = dataBase;
            _serviceProvider = serviceProvider;
        }

        public event Action DataBaseChanged;

        public string Name { get; }

        public void LoadData()
        {
            _dataBase = (CompaniesDataBase)_mySqlDataBase.LoadAllData();
        }

        public void UpdateLoadDataBase()
        {            
            UpdateDataBase();
        }

        public void CreateNewCompany(Company newCompany)
        {
            if (_dataBase.TryAddNewCompany(newCompany))
            {
                _mySqlDataBase.Execute(new MySqlDBDataWriter(), newCompany);
                UpdateDataBase();
            }
        }

        public void DeleteCompany(Company deletedCompany) 
        {
            if (_dataBase.TryDeleteCompany(deletedCompany))
            { 
                _mySqlDataBase.Execute(new MySqlDBDataDeleter(), deletedCompany);
                UpdateDataBase();
            }
        }

        public void UpdateCompanyData(Company company)
        {
            if (_dataBase.UpdateCompanyData(company))
            { 
                _mySqlDataBase.Execute(new MySqlDBDataUpdater(), company);
                UpdateDataBase();
            }
        }

        public void CreateNewCompanyObject(CompanyObject newCompanyObject)
        {
            if (_dataBase.AddNewCompanyObject(newCompanyObject))
            {
                _mySqlDataBase.Execute(new MySqlDBDataWriter(), newCompanyObject);
                UpdateDataBase();
            }
        }

        public void DeleteCompanyObject(CompanyObject objectToDelete)
        {
            if (_dataBase.DeleteCompanyObject(objectToDelete))
            {
                _mySqlDataBase.Execute(new MySqlDBDataDeleter(), objectToDelete);
                UpdateDataBase();
            }
        }

        public void UpdateCompanyObject(CompanyObject companyObject)
        {
            if (_dataBase.UpdateCompanyObject(companyObject))
            { 
                _mySqlDataBase.Execute(new MySqlDBDataUpdater(), companyObject);
                UpdateDataBase();
            }
        }
        public IEnumerable<Company> GetAllCmpanies()
        {
            return _dataBase.GetAllCompanies();
        }

        private void UpdateDataBase()
        {
            DataBaseChanged?.Invoke();                        
        }

        public void CopyCompaniesToEF()
        {
            var dbContext = _serviceProvider.GetService<DelitaDbContext>();
            //dbContext.ReturnProtocols.ExecuteDelete();
            //dbContext.Objects.ExecuteDelete();
            //dbContext.Traders.ExecuteDelete();
            //dbContext.Companies.ExecuteDelete();
            foreach (var item in _dataBase.companies)
            {

                var newCompany = dbContext.Companies.Local.FirstOrDefault(c => c.Name == item.Name);
                if (newCompany == null)
                {
                    newCompany = new DBDelitaTrade.Infrastructure.Data.Models.Company
                    {
                        Name = item.Name,
                        Type = item.Type,
                        Bulstad = item.Bulstad,
                    };
                }
                else
                {
                    newCompany.Type = item.Type;
                    newCompany.Bulstad = item.Bulstad;
                }
                foreach (var obj in item.GetAllCompanyObjects())
                {
                    var trader = dbContext.Traders.Local.Where(t => t.Name == obj.Trader).FirstOrDefault();
                    if (trader == null) 
                    {
                        trader = new DBDelitaTrade.Infrastructure.Data.Models.Trader { Name = obj.Trader };
                        dbContext.Traders.Add(trader);
                    }
                    

                    var newObject = dbContext.Objects.Local.Where(o => o.Name == obj.Name).FirstOrDefault();
                    if (newObject == null)
                    {
                        newObject = new DBDelitaTrade.Infrastructure.Data.Models.CompanyObject
                        {
                            Name = obj.Name,
                            Address = obj.Adrress,
                            Trader = trader,
                            Company = newCompany,
                            IsBankPay = obj.BankPay
                        };
                    }
                    else
                    {
                        newObject.Address = obj.Adrress;
                        newObject.IsBankPay = obj.BankPay;
                        newObject.Company = newCompany;
                        newObject.Trader = trader;
                    }                     
                    newCompany.Objects.Add(newObject);

                }
                
                dbContext.Add(newCompany);                
            }
            dbContext.SaveChanges();
        }
    }
}
