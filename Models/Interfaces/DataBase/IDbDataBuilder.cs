using DelitaTrade.Components.ComponentsCommands;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDbDataBuilder : IDbReadableData
    {
        IDBData GetData(string[] parameters);

        MySqlParameter[] GetParameters();
        
        void AddParameter(MySqlParameter parameter);

        void ClearParameters();

        abstract Type GetDbType();
    }
}
