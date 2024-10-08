namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBProvider
    {
        void Execute(IDBExecute dBExecute, IDBData company);
        void Execute(IDBExecute dBExecute, IDBData companyObject, params string[] parameterNames);
        IDBDataParse LoadAllData();
    }
}
