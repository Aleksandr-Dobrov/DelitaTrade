using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;
using DelitaTrade.Models.ReturnProtocolSQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Builder.DataBase
{
    public class ProductBuilder : DBBaseBuilder
    {
        public override MySqlReadCommand[] ReadCommands => [MySqlReadCommand.AllProducts];

        public override IDBData GetData(string[] parameters)
        {
            return new SqlProduct(int.Parse(parameters[0]), parameters[1], parameters[2]);
        }

        public override Type GetDbType()
        {
            return typeof(SqlProduct);
        }
    }
}
