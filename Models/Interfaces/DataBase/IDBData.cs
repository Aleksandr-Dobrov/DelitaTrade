using System.Configuration;

namespace DelitaTrade.Models.Interfaces.DataBase
{
    public interface IDBData
    {
        string Parameters { get; }
        string Data { get; }
        string Procedure { get; }
        int NumberOfAdditionalParameters { get; }        
    }
}
