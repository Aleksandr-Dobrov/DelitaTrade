using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;
using MySql.Data.MySqlClient;
using System.Collections;

namespace DelitaTrade.Models.DataBases
{
    public class DbManyObjService : IDBDataParser
    {
        private HashSet<IDBData> _dbDataObjects;
        private MySqlReadCommand[] _readCommand;
        private IDbDataBuilder _dbDataBuilder;

        public DbManyObjService(IDbDataBuilder dbDataBuilder)
        {
            _dbDataBuilder = dbDataBuilder;
            _dbDataObjects = new HashSet<IDBData>();
        }

        public MySqlReadCommand[] ReadCommands => _dbDataBuilder.ReadCommands;
        public MySqlParameter[] Parameters => _dbDataBuilder.GetParameters();
        public IDbDataBuilder DataBuilder => _dbDataBuilder;
        public int Count => _dbDataObjects.Count;

        public bool ContainsKey(IDBData dBData)
        {
            return _dbDataObjects.Contains(dBData);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in _dbDataObjects)
            {
                yield return item;
            }
        }

        public void Parse(string inputData)
        {
            string[] result = inputData.Split("-=-");
            IDBData dBData = _dbDataBuilder.GetData(result);
            if (_dbDataObjects.Contains(dBData) == false)
            {
                _dbDataObjects.Add(dBData);
            }
        }

        public bool AddData(IDBData data)
        {
            return _dbDataObjects.Add(data);
        }

        public bool RemoveData(IDBData data)
        {
            return _dbDataObjects.Remove(data);
        }

        public IDBData GetObject(IDBData data)
        {
            IDBData obj;
            _dbDataObjects.TryGetValue(data, out obj);
            return obj;
        }

        IEnumerator<IDBData> IEnumerable<IDBData>.GetEnumerator() 
        {
            return _dbDataObjects.GetEnumerator(); 
        }
    }
}
