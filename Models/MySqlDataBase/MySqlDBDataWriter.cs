using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.Loggers;
using Devart.Data.MySql;
using System.Data;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBDataWriter : IDBExecute
    {
        private MySqlCommand _writer;
        private IDBDelitaConnection _connection;
        private List<string> _parameters;
        private List<string> _values;
        private int _countOfParameters;

        public void Execute(IDBDelitaConnection connection, IDBData dBData)
        {
            try 
            {
                _connection = connection;
                SetCommand(dBData.Procedure);
                SetParametrs(dBData);
                SetValues(dBData);
                _writer.Parameters.AddRange(SetSqlParametrs());
                _writer.Connection.Open();
                var result = _writer.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
            finally
            {
                _writer.Connection.Close();                
            }
        }

        public void Execute(IDBDelitaConnection connection, IDBData dBData, params string[] parameterNames)
        {
            try
            {
                _connection = connection;
                SetCommand(dBData.Procedure);
                SetParametrs(dBData);
                SetValues(dBData, parameterNames);
                _writer.Parameters.AddRange(SetSqlParametrs());
                _writer.Connection.Open();
                var result = _writer.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
            finally
            {
                _writer.Connection.Close();
            }
        }

        protected virtual void SetCommand(string procedure)
        {
            _writer = new MySqlCommand(procedure, _connection.MySqlConnection);
            _writer.CommandType = CommandType.StoredProcedure;
        }

        private void SetParametrs(IDBData dBData)
        {
            _parameters = [.. dBData.Parameters.Split("-=-")];
        }

        private void SetValues(IDBData dBData)
        {
            _values = [.. dBData.Data.Split("-=-")];
            SetCountOfParameters(dBData);
        }

        private void SetValues(IDBData dBData, params string[] parameterNames)
        {
            _values = [..parameterNames];
            foreach (string value in dBData.Data.Split("-=-"))
            {
                _values.Add(value);
            }
            SetCountOfParameters(dBData, parameterNames.Length);
        }

        private MySqlParameter[] SetSqlParametrs()
        {
            List<MySqlParameter> sqlParameters = [];
            for (int i = 0; i < _countOfParameters; i++)
            {
                sqlParameters.Add(new MySqlParameter(_parameters[i], _values[i]));
            }
            return sqlParameters.ToArray();
        }

        protected virtual void SetCountOfParameters(IDBData dBData)
        {
            _countOfParameters = dBData.Data.Split("-=-").Length;
        }
        protected virtual void SetCountOfParameters(IDBData dBData, int numberOfAdditionalParams)
        {
            _countOfParameters = dBData.Data.Split("-=-").Length + numberOfAdditionalParams;
        }
    }
}
