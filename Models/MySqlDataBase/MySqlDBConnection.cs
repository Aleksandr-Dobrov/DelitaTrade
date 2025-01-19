using DelitaTrade.Models.Interfaces.DataBase;
using Microsoft.Extensions.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBConnection : IDBDelitaConnection
    {
        private MySqlConnection _mySqlConnection;

        public MySqlConnection MySqlConnection => _mySqlConnection;

        public void CreateConnectionToDB(IConfiguration configuration)
        {
            var section = configuration.GetSection("MySqlConnection");
            var host = section.GetValue(typeof(string), "host") as string;
            var port = section.GetValue(typeof(string), "port") as string;
            var userId = section.GetValue(typeof(string), "userId") as string;
            var password = section.GetValue(typeof(string), "password") as string;
            var dataBase = section.GetValue(typeof(string), "database") as string;
            var sslMode = section.GetValue(typeof(string), "sslMode") as string;
            _mySqlConnection = new MySqlConnection($"server={host};port={port};user id={userId}; password={password}; database={dataBase}; SslMode={sslMode}");
        }
    }
}
