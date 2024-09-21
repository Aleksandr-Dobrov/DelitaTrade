using DelitaTrade.Interfaces.ReturnProtocol;
using DelitaTrade.Models.Loggers;
using System.IO;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ReturnProtocolServices
    {
        private string _protocolIdFilePath;

        private string _userName;

        private int _protocolId;

        private ReturnProtocolProductDataService _productDataService;
        private ReturnProtocolDataServices _protocolDataServices;
        private ReturnProtocolDelita _curentReturnProtocol;
        private Stack<ReturnProtocolDelita> _deletedReturnProtocols;

        public ReturnProtocolServices()
        {
            _protocolDataServices = new ReturnProtocolDataServices();
            _deletedReturnProtocols = new Stack<ReturnProtocolDelita>();
        }

        private string SetProtocolId()
        {
            if (File.Exists(_protocolIdFilePath))
            {                
                _protocolId = int.Parse(File.ReadAllText(_protocolIdFilePath));
            }
            else
            {
                _protocolId = 0;
            }

            _protocolId++;
            File.WriteAllText(_protocolIdFilePath,_protocolId.ToString());           
            return $"ПВ{_protocolId + 100000}";
        }

        public void CreateNewReturnProtocol(Company company, CompanyObject companyObject)
        {
            try 
            {
                LoadCurentReturnProtocolDataService(new ReturnProtocolDelita(company, companyObject, _userName, SetProtocolId()));                
                _protocolDataServices.SaveReturnProtocolToDataBase(_curentReturnProtocol);
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        public int DeletedProtocolsCount => _deletedReturnProtocols.Count;

        public IReturnProtokolProduct ReturnProtokolProductDataService => _productDataService;

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
                LoadCurentReturnProtocolDataService(_deletedReturnProtocols.Pop());                
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
                LoadCurentReturnProtocolDataService(_protocolDataServices.LoadReturnaProtocolFromDataBase(returnProtocolId));                
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        private void LoadCurentReturnProtocolDataService(ReturnProtocolDelita loadedReturnProtocol)
        {
            _curentReturnProtocol = loadedReturnProtocol;
            _productDataService = new ReturnProtocolProductDataService(_curentReturnProtocol);
        }
    }
}
