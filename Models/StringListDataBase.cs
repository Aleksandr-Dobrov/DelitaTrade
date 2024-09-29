using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.Interfaces;
using System.Collections;

namespace DelitaTrade.Models
{
    public class StringListDataBase : IEnumerable<string>, IDeleteColectionStringItem
    {
        private readonly DataStringProvider _dataStringProvider;

        private List<string> _colection;

        private readonly string _title;

        public StringListDataBase(string filePath, string title)
        {
            _title = title;
            _dataStringProvider = new DataStringProvider(filePath);
            _colection = _dataStringProvider.TryLoadData().ToList();
            ColectionChainge += () => { };
        }

        public event Action ColectionChainge;

        public string Title => _title;

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

        public void DeleteItem(string item)
        {
            _colection.Remove(item);
            Save();
            ColectionChainge();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Save()
        {
            _dataStringProvider.SaveAllToDataBase(this);
        }
    }
}
