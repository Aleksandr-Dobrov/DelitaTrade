using DelitaTrade.Models.Interfaces.DataBase;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDbReadProvider
    {
        MySqlDBConnection _connection;

        public MySqlDbReadProvider(MySqlDBConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _connection.CreateConnectionToDB(configuration);
        }

        public string[] ReadData(MySqlReadCommand command)
        {
            MySqlDBReader reader = new MySqlDBReader(command, _connection);

            return reader.GetAllData().ToArray();
        }

        public string[] ReadData(MySqlReadCommand command, params MySqlParameter[] parameters)
        {
            MySqlDBReader reader = new MySqlDBReader(command, _connection);

            return reader.GetAllData(parameters).ToArray();
        }
    }
}
