using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.Interfaces;
using System.Collections;

namespace DelitaTrade.Models
{
    public class StringListDataBase : IEnumerable<string>, IDeleteColectionStringItem
    {
        private List<string> _colection;

        private readonly string _title;

        private readonly DataStringProvider _dataStringProvider;

        public StringListDataBase(string filePath, string title)
        {
            _title = title;
            _dataStringProvider = new DataStringProvider(filePath);
            _colection = _dataStringProvider.TryLoadData().ToList();
            ColectionChainge += () => { };
        }

        public string Title => _title;

        public event Action ColectionChainge;

        private void Save()
        {
            _dataStringProvider.SaveAllToDataBase(this);
        }

        public void Add(string item)
        {
            _colection.Add(item);
            Save();
            ColectionChainge();
        }

        public void Remove(string item)
        {
            _colection.Remove(item);
            Save();
            ColectionChainge();
        }

        public bool Contains(string item)
        {
            return _colection.Contains(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < _colection.Count; i++)
            {
                yield return _colection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                

        public void DeleteItem(string item)
        {
            _colection.Remove(item);
            Save();
            ColectionChainge();
        }
    }
}
