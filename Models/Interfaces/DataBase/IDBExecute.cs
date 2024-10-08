namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBExecute
    {
        void Execute(IDBDelitaConnection connection, IDBData dBData);
        void Execute(IDBDelitaConnection connection, IDBData dBData, params string[] parameterNames);
    }
}
