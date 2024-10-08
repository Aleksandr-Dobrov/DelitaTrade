using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.Interfaces.ReturnProtocol;
using System.IO;
using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ReturnProtocolDataServices
    {
        const string IdFilePath = "../../../ReturnProtocol/IdLastNumber.dsc";

        [DataMember]
        private HashSet<string> _returnProtocolIdCollection;
        [DataMember]
        private ReturnProtocolDataBase _returnProtocolDataBase;
        private IdDataGenerator _idGenerator;

        private IDelitaDataBase<ReturnProtocolDelita> _dataBase;
        private IDelitaDataBase<HashSet<string>> _returnProtocolIds;
        private IDelitaDataBase<ReturnProtocolDataBase> _returnProtocolDataBaseProvider;

        private string _returnProtocolIdsFilePath = "../../../ReturnProtocol/ReturnProtocolIds.xml";
        private string _dataBaseDirectoryPath = "../../../ReturnProtocol/ReturnProtocolsDataBase/";

        public ReturnProtocolDataServices(int code)
        {            
            _dataBase = new XmlDataBase<ReturnProtocolDelita>();
            _returnProtocolIds = new XmlDataBase<HashSet<string>>();
            _returnProtocolDataBaseProvider = new XmlDataBase<ReturnProtocolDataBase>();           
            _returnProtocolIds.Path = _returnProtocolIdsFilePath;
            _returnProtocolDataBaseProvider.Path = ReturnProtocolDataBase.savePath;
            _idGenerator = new IdDataGenerator(IdFilePath, 5, code);
            TryLoadReturnProtocolIds();
            TryLoadReturnProtocolDataBase();
            _returnProtocolDataBase.DataBaseChange += SafeReturnProtocolDataBase;
        }

        public ISearchProvider SearchProvider => _returnProtocolDataBase;

        public ReturnProtocolDataBase ReturnProtocolDataBase => _returnProtocolDataBase;

        public string GetProtocolId(int code) => _idGenerator.GetId(code);

        public void SafeReturnProtocolDataBase()
        {
            _returnProtocolDataBaseProvider.SaveAllData(ReturnProtocolDataBase);
        }

        public void UpdateReturnProtocolProductData(ReturnProtocolDelita returnProtocolDelita)
        {
            if (_returnProtocolIdCollection.Contains(returnProtocolDelita.ID))
            {
                _dataBase.Path = DataBaseFilePathSet(returnProtocolDelita);
                _dataBase.SaveAllData(returnProtocolDelita);
            }
            else 
            {
                throw new ArgumentException("Return protocol is not exists in data base!");
            }
        }

        public void SaveReturnProtocolToDataBase(ReturnProtocolDelita returnProtocolDelita)
        {
            if (_returnProtocolIdCollection.Contains(returnProtocolDelita.ID) == false)
            {
                _returnProtocolIdCollection.Add(returnProtocolDelita.ID);
                _returnProtocolIds.SaveAllData(_returnProtocolIdCollection);
                _returnProtocolDataBase.AddReturnProtocol(returnProtocolDelita);
            }
            else
            {
                _returnProtocolDataBase.UpdateReturnProtocol(returnProtocolDelita, returnProtocolDelita);
            }
            
            _dataBase.Path = DataBaseFilePathSet(returnProtocolDelita);
            _dataBase.SaveAllData(returnProtocolDelita);
        }

        public ReturnProtocolDelita LoadReturnaProtocolFromDataBase(string returnProtocolId)
        {
            if (_returnProtocolIdCollection.Contains(returnProtocolId))
            {
                _dataBase.Path = DataBaseFilePathSet(returnProtocolId);
                return _dataBase.LoadAllData();
            }
            else
            {
                throw new ArgumentException("Return protocol is not exists in data base!");
            }
        }

        public void DeleteReturnProtocolFromDataBase(ReturnProtocolDelita returnProtocolDelita)
        {
            ArgumentNullException.ThrowIfNull(returnProtocolDelita);

            if (_returnProtocolIdCollection.Contains(returnProtocolDelita.ID))
            {
                DeleteReturnProtocolFile(DataBaseFilePathSet(returnProtocolDelita));
                _returnProtocolIdCollection.Remove(returnProtocolDelita.ID);
                _returnProtocolDataBase.DeleteReturnProtocol(returnProtocolDelita);
                _returnProtocolIds.SaveAllData(_returnProtocolIdCollection);
            }
            else 
            {
                throw new ArgumentException("Return protocol is not exists in data base.");
            }
        }

        private string DataBaseFilePathSet(ReturnProtocolDelita returnProtocolDelita)
        {
            return $"{_dataBaseDirectoryPath}{returnProtocolDelita.ID}.xml";
        }

        private string DataBaseFilePathSet(string returnProtocolId)
        {
            return $"{_dataBaseDirectoryPath}{returnProtocolId}.xml";
        }

        private void DeleteReturnProtocolFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else 
            {
                throw new ArgumentNullException("Return protocol file not found!");
            }
        }

        private void TryLoadReturnProtocolDataBase()
        {
            if (File.Exists(ReturnProtocolDataBase.savePath))
            {
                _returnProtocolDataBase = _returnProtocolDataBaseProvider.LoadAllData();
            }
            else
            {
                _returnProtocolDataBase = new ReturnProtocolDataBase();
            }
        }

        private void TryLoadReturnProtocolIds()
        {
            if (File.Exists(_returnProtocolIdsFilePath))
            {
                _returnProtocolIdCollection = _returnProtocolIds.LoadAllData();
            }
            else 
            {
                _returnProtocolIdCollection = new HashSet<string>();
            }
        }
    }
}
