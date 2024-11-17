using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models.Builder.DataBase
{
    public class DBPayDeskBuilder : DBBaseBuilder
    {
        public override MySqlReadCommand[] ReadCommands => [MySqlReadCommand.AllPayDesks];

        public override IDBData GetData(string[] parameters)
        {
            PayDesk payDesk = new PayDesk(int.Parse(parameters[0]));
            string[] banknotes = parameters[2].Trim(['[',']']).Split("][");
            foreach (string banknote in banknotes)
            {
                string[] valueCount = banknote.Split(" => ");
                if (int.Parse(valueCount[1]) > 0) 
                {
                    payDesk.AddMoney(valueCount[0], int.Parse(valueCount[1]));
                }
            }

            if (payDesk.Amount == decimal.Parse(parameters[1]))
            {
                return payDesk;
            }
            else
            {
                throw new InvalidOperationException($"Can't parse correct pars money from data base. Difference is:{decimal.Parse(parameters[1]) - payDesk.Amount}");
            }
        }

        public override Type GetDbType()
        {
            return typeof(PayDesk);
        }
    }
}
