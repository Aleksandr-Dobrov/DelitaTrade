using DelitaTrade.Models.Loggers;
using Devart.Data.MySql;
using System.Data;

namespace DelitaTrade.Models.MySqlDataBase
{
    public enum MySqlReadCommand 
    {
        AllCompanies,
        AllObjects,
    }


    public class MySqlDBReader
    {
        private Dictionary<MySqlReadCommand, string> _readCommands = new()
        {
            [MySqlReadCommand.AllCompanies] = "get_companies_with_out_objects",
            [MySqlReadCommand.AllObjects] = "get_all_objects"
        };
        private Dictionary<MySqlReadCommand, int> _readData = new()
        {
            [MySqlReadCommand.AllCompanies] = 3,
            [MySqlReadCommand.AllObjects] = 4
        };
        private MySqlCommand _reader;
        private MySqlReadCommand _command;

        public MySqlDBReader(MySqlReadCommand command, MySqlDBConnection connection)
        {
            _command = command;
            _reader = new MySqlCommand(_readCommands[_command], connection.MySqlConnection);
            _reader.CommandType = CommandType.StoredProcedure;
        }

        public IEnumerable<string> GetAllData()
        {
            List<string> data = new List<string>();
            try
            {
                _reader.Connection.Open();
                MySqlDataReader reader = _reader.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(reader.GetString(_readData[_command]));
                }
            }
            catch (MySqlException ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error).Log(ex,Logger.LogLevel.Error);
            }
            finally
            {
                _reader.Connection.Close();
            }
            return data;
        }
    }
}
