namespace DelitaTrade.Models
{
    public interface IDataBase<T>
    {
        public string Path { set; }

        public event Action<string> UsedDefaultPath;
        public void SaveAllData(T data);

        public T LoadAllData();
    }
}
