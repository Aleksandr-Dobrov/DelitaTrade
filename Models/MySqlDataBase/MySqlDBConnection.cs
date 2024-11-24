using DelitaTrade.Models.Interfaces.DataBase;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBConnection : IDBDelitaConnection
    {
        private const string host= "127.0.0.1";
        private const string port = "3306";
        private const string userId = "root";
        private const string password = "10052016AdiCA0527CK.Audi";
        private const string database = "delita_db";
        private const string sslMode = "Required";
        private MySqlConnection _mySqlConnection;

        public MySqlConnection MySqlConnection => _mySqlConnection;

        public void CreateConectionToDB()
        {            
            _mySqlConnection = new MySqlConnection($"server={host};port={port};user id={userId}; password={password}; database={database}; SslMode={sslMode}");             
        }
    }
}
