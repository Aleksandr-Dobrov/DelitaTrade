using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Builder.DataBase
{
    public abstract class DBBaseBuilder : IDbDataBuilder
    {
        private List<MySqlParameter> _parameters = new List<MySqlParameter>();

        public abstract MySqlReadCommand[] ReadCommands { get; }

        public void AddParameter(MySqlParameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void ClearParameters()
        {
            _parameters?.Clear();
        }

        public abstract IDBData GetData(string[] parameters);
        public abstract Type GetDbType();

        public MySqlParameter[] GetParameters()
        {
            return [.. _parameters];
        }
    }
}
