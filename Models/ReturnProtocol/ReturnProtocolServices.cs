using DelitaTrade.Models.Interfaces.ReturnProtocol;
using DelitaTrade.Models.Loggers;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ReturnProtocolServices
    {
        private ReturnProtocolProductDataService _productDataService;
        private ReturnProtocolDataServices _protocolDataServices;
        private ReturnProtocolDelita _curentReturnProtocol;
        private Stack<ReturnProtocolDelita> _deletedReturnProtocols;
        
        private string _userName;
        private int _idCode;

        public ReturnProtocolServices()
        {
            _protocolDataServices = new ReturnProtocolDataServices(_idCode);
            _deletedReturnProtocols = new Stack<ReturnProtocolDelita>();            
        }

        public IReturnProtokolProduct ReturnProtokolProductDataService => _productDataService;

        public ISearchProvider SearchProvider => _protocolDataServices.SearchProvider;

        public int DeletedProtocolsCount => _deletedReturnProtocols.Count;

        public void UpdateProductData()
        {
            try
            {               
                _protocolDataServices.UpdateReturnProtocolProductData(_curentReturnProtocol);
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        public void CreateNewReturnProtocol(ICompany company, ICompanyObject companyObject)
        {
            try 
            {
                SetCurentReturnProtocolDataService(new ReturnProtocolDelita(company, companyObject, _userName, _protocolDataServices.GetProtocolId(_idCode)));                
                _protocolDataServices.SaveReturnProtocolToDataBase(_curentReturnProtocol);
               
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        public void DeleteReturnProtocol()
        {
            try 
            {
                _protocolDataServices.DeleteReturnProtocolFromDataBase(_curentReturnProtocol);
                _deletedReturnProtocols.Push(_curentReturnProtocol);
                _curentReturnProtocol = null;
                _productDataService = null;
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        public void RestoreDelitedReturnProtocol()
        {
            try 
            {
                SetCurentReturnProtocolDataService(_deletedReturnProtocols.Pop());                
                _protocolDataServices.SaveReturnProtocolToDataBase(_curentReturnProtocol);
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        public void LoadReturnProtocol(string returnProtocolId)
        {
            try
            {
                SetCurentReturnProtocolDataService(_protocolDataServices.LoadReturnaProtocolFromDataBase(returnProtocolId));                
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        private void SetCurentReturnProtocolDataService(ReturnProtocolDelita loadedReturnProtocol)
        {
            _curentReturnProtocol = loadedReturnProtocol;
            _productDataService = new ReturnProtocolProductDataService(_curentReturnProtocol);
        }
    }
}
