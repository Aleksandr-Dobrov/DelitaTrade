using DelitaTrade.Models.MySqlDataBase;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBDataParser : IEnumerable<IDBData>
    {
        int Count { get; }
        bool ContainsKey(IDBData dBData);
        void Parse(string inputData);
        IDBData GetObject(IDBData data);
        bool AddData(IDBData data);
        bool RemoveData(IDBData data);
        MySqlReadCommand[] ReadCommands { get; } 
        MySqlParameter[] Parameters { get; }
        IDbDataBuilder DataBuilder { get; }
    }
}
