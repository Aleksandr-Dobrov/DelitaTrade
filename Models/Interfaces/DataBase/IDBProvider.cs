using DelitaTrade.Models.Interfaces.DataBase;
using MySql.Data.MySqlClient;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBProvider
    {
        void Execute(IDBExecuter dBExecute, IDBData company);
        void Execute(IDBExecuter dBExecute, IDBData companyObject, params string[] parameterNames);
        IDBDataParser LoadAllData();
        void LoadAllData(ref IDBDataParser dBDataParse);
    }
}