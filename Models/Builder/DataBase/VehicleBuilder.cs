using DelitaTrade.Models.DataBases.DayReportDataBase;
using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.Builder.DataBase
{
    public class VehicleBuilder : DBBaseBuilder
    {
        private List<MySqlParameter> _parameters;

        public VehicleBuilder()
        {
            _parameters = new List<MySqlParameter>();
        }

        public override MySqlReadCommand[] ReadCommands => [MySqlReadCommand.AllVehicles];

        public override IDBData GetData(string[] parameters)
        {
            return new Vehicle(parameters[0], parameters[1]);
        }

        public override Type GetDbType()
        {
            return typeof(Vehicle);
        }
    }
}
