namespace DelitaTrade.Models.MySqlDataBase
{
    public class MySqlDBDataUpdater : MySqlDBDataWriter
    {
        private const string _procedure = "update";
        
        protected override void SetCommand(string procedure)
        {
            string[] updateProcedure = procedure.Split("_");

            base.SetCommand($"{_procedure}_{updateProcedure[1]}_{updateProcedure[2]}");
        }
    }
}
