using System.IO;

namespace DelitaTrade.Models.DataProviders
{
    public class DataStringProvider
    {
        private string[] _items;

        private readonly string _filePath;

        public DataStringProvider(string filePath)
        {
            _filePath = filePath;            
        }

        public void SaveAllToDataBase(IEnumerable<string> values)
        {
            if (!File.Exists(_filePath))
            {
                Directory.CreateDirectory(DirectoryPath());                
            }

            using (StreamWriter saveValues = new StreamWriter(_filePath))
            {                
                foreach (var value in values)
                {
                    saveValues.WriteLine($"{value}");
                }
            }
        }

        public DataStringProvider TryLoadData()
        {
            if (File.Exists(_filePath))
            {
                _items = File.ReadAllLines(_filePath);
                return this;
            }
            else
            {
                _items = new string[0];
                return this;
            }
        }

        public List<string> ToList() 
        {
            return _items.ToList();
        }

        public HashSet<string> ToHashSet() 
        {
            return _items.ToHashSet();
        }

        private string DirectoryPath()
        {
            int lastIndex = _filePath.LastIndexOf('/');
            return _filePath.Substring(0, lastIndex);
        }
    }
}
