using System.IO;
using System.Runtime.Serialization;

namespace DelitaTrade.Models.ReturnProtocol
{
    public class ReturnProtocolDataServices
    {
        [DataMember]
        private HashSet<string> _returnProtocolIdCollection;

        private string _returnProtocolIdsFilePath = "../../../ReturnProtocol/ReturnProtocolIds.xml";

        private string _dataBaseDirectoryPath = "../../../ReturnProtocol/ReturnProtocolsDataBase/";

        private IDataBase<ReturnProtocolDelita> _dataBase;

        private IDataBase<HashSet<string>> _returnProtocolIds;

        public ReturnProtocolDataServices()
        {            
            _dataBase = new XmlDataBase<ReturnProtocolDelita>();
            _returnProtocolIds = new XmlDataBase<HashSet<string>>();
            _returnProtocolIds.Path = _returnProtocolIdsFilePath;
            TryLoadReturnProtocolIds();
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

        public void SaveReturnProtocolToDataBase(ReturnProtocolDelita returnProtocolDelita)
        {
            if (_returnProtocolIdCollection.Contains(returnProtocolDelita.ID) == false)
            {
                _returnProtocolIdCollection.Add(returnProtocolDelita.ID);
                _returnProtocolIds.SaveAllData(_returnProtocolIdCollection);
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
                _returnProtocolIds.SaveAllData(_returnProtocolIdCollection);
            }
            else 
            {
                throw new ArgumentException("Return protocol is not exists in data base.");
            }
        }
    }
}
