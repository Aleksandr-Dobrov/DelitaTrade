using DelitaTrade.Models.Interfaces.DataBase;
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

        public MySqlDbReadProvider(MySqlDBConnection connection)
        {
            _connection = connection;
            _connection.CreateConectionToDB();
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
