using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBDelitaConnection
    {
        public MySqlConnection MySqlConnection { get; }
        public void ConectToDB();
    }
}
