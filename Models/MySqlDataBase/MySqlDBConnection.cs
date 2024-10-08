using DelitaTrade.Models.Interfaces.DataBase;
using Devart.Data.MySql;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBConnection : IDBDelitaConnection
    {

        private readonly MySqlConnection _mySqlConnection;

        public MySqlDBConnection()
        {
            _mySqlConnection = new MySqlConnection();
        }

        public MySqlConnection MySqlConnection => _mySqlConnection;

        public void ConectToDB()
        {
            _mySqlConnection.Host = "127.0.0.1";
            _mySqlConnection.Port = 3306;
            _mySqlConnection.UserId = "root";
            _mySqlConnection.Password = "10052016AdiCA0527CK.Audi";
            _mySqlConnection.Database = "delita_db";
        }
    }
}
