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
    public class DBDayReportBuilder : DBBaseBuilder
    {
        private List<MySqlParameter> _parameters;

        public DBDayReportBuilder()
        {
            _parameters = new List<MySqlParameter>();
        }

        public override MySqlReadCommand[] ReadCommands => [MySqlReadCommand.AllDayReports];
                
        public override IDBData GetData(string[] parameters)
        {
            return new DayReport(parameters[0] ,parameters[1], decimal.Parse(parameters[2]), decimal.Parse(parameters[3]), decimal.Parse(parameters[4]),
                decimal.Parse(parameters[5]), double.Parse(parameters[6]), decimal.Parse(parameters[7]), parameters[8],
                string.Join('-', parameters[9].Split('-').Reverse()), int.Parse(parameters[10]));
        }

        public override Type GetDbType()
        {
            return typeof(DayReport);
        }
    }
}
