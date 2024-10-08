using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBDataParse
    {
        void Parse(string inputData);
        MySqlReadCommand[] ReadCommands { get; } 
    }
}
