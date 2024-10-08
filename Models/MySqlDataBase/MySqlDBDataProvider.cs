using DelitaTrade.Models.Interfaces.DataBase;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBDataProvider : IDBProvider
    {
        private readonly IDBDataParse _dataBase;
        private readonly MySqlDBConnection _connection;
        private MySqlDBReader _reader;
                
        public MySqlDBDataProvider(MySqlDBConnection connection, IDBDataParse dataBase)
        {
            _connection = connection;
            _connection.ConectToDB();
            _dataBase = dataBase;
        }

        public IDBDataParse LoadAllData()
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

        public void Execute(IDBExecute dBExecut, IDBData company)
        {            
            dBExecut.Execute(_connection, company);         
        }

        public void Execute(IDBExecute dBExecute, IDBData companyObject, params string[] parameterNames)
        {
            dBExecute.Execute(_connection, companyObject, parameterNames);
        }
    }
}
