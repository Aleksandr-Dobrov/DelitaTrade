using DelitaTrade.Models.Interfaces.DataBase;

namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBDataDeleter : MySqlDBDataWriter
    {
        private const string _procedure = "delete";
        private const int _countOfParams = 1;
        
        protected override void SetCommand(string procedure)
        {
            string[] deleteProcedure = procedure.Split('_');

            base.SetCommand($"{_procedure}_{deleteProcedure[1]}");
        }

        protected override void SetCountOfParameters(IDBData dBData)
        {            
            base.SetCountOfParameters(dBData, ((dBData.Data.Split("-=-").Length) * -1) + _countOfParams + dBData.NumberOfAdditionalParameters);
        }

        protected override void SetCountOfParameters(IDBData dBData, int numberOfAdditionalParams)
        {
            base.SetCountOfParameters(dBData, ((dBData.Data.Split("-=-").Length) * -1) + _countOfParams + numberOfAdditionalParams + dBData.NumberOfAdditionalParameters);
        }
    }
}
