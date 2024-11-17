using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models.Builder.DataBase
{
    public class DBInvoiceInDayReportBuilder : DBBaseBuilder
    {
        public override MySqlReadCommand[] ReadCommands => [MySqlReadCommand.AllInvoicesInDayReport];
        
        public override IDBData GetData(string[] parameters)
        {
            return new Invoice(int.Parse(parameters[0]), int.Parse(parameters[1]), parameters[2], parameters[3], parameters[4], 
                    parameters[5], parameters[6], decimal.Parse(parameters[7]), decimal.Parse(parameters[8]), double.Parse(parameters[9]));
        }

        public override Type GetDbType()
        {
            return typeof(Invoice);
        }
    }
}
