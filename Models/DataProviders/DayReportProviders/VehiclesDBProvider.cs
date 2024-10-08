using DelitaTrade.Models.DataBases.DayReport;
using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models.DataProviders.DayReportProviders
{
    public class VehiclesDBProvider : IDBDataParse 
    {
        private Dictionary<int, IDBIdData> _data;

        public VehiclesDBProvider()
        {
            _data = new Dictionary<int, IDBIdData>();
        }

        public MySqlReadCommand[] ReadCommands => throw new NotImplementedException();

        public void Parse(string inputData)
        {

        }
    }
}
