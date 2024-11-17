using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDbReadableData
    {
        MySqlReadCommand[] ReadCommands { get; }
    }
}
