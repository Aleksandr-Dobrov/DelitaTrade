using DelitaTrade.Models.Interfaces.DataBase;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBDataProvider : IDBProvider
    {
        private readonly IDBDataParser _dataBase;
        private readonly MySqlDBConnection _connection;
        private MySqlDBReader _reader;
                
        public MySqlDBDataProvider(MySqlDBConnection connection, IDBDataParser dataBase)
        {
            _connection = connection;
            _connection.CreateConectionToDB();
            _dataBase = dataBase;
        }

        public IDBDataParser LoadAllData()
        {
            foreach (var command in _dataBase.ReadCommands)
            {
                _reader = new MySqlDBReader(command, _connection);
                foreach (var obj in _reader.GetAllData())
                {
                    _dataBase.Parse(obj);
                }
            }
            return _dataBase;
        }

        public void LoadAllData(ref IDBDataParser dBDataParse)
        {
            foreach (var command in dBDataParse.ReadCommands)
            {
                _reader = new MySqlDBReader(command, _connection);
                foreach (var obj in _reader.GetAllData(dBDataParse.Parameters))
                {
                    dBDataParse.Parse(obj);
                }
            }
        }
                
        public void Execute(IDBExecuter dBExecut, IDBData company)
        {            
            dBExecut.Execute(_connection, company);         
        }

        public void Execute(IDBExecuter dBExecute, IDBData companyObject, params string[] parameterNames)
        {
            dBExecute.Execute(_connection, companyObject, parameterNames);
        }
    }
}
