using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace DelitaTrade.Models
{
    public class XmlDataBase<T> : IDelitaDataBase<T>
    {
        private string _defautPath = $"../../../XmlDataBase/XmlData{DateTime.Now.Date:dd-MM-yyyy}.xml";
        private string _path;

        public XmlDataBase()
        {
            UsedDefaultPath += (string obj) => { };
        }
        
        public event Action<string> UsedDefaultPath;

        public string Path
        {
            set
            {
                _path = CreateFilePath(value);
            }
        }

        public T LoadAllData()
        {
            try 
            {
                var fileStream = new FileStream(_path, FileMode.Open);
                var reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
                var serializer = new DataContractSerializer(typeof(T));
                T SerializebleObject = (T)serializer.ReadObject(reader, true);
                reader.Close();
                fileStream.Close();
                return SerializebleObject;
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Data not exists");
            }
        }

        public void SaveAllData(T data)
        {
            var serializerr = new DataContractSerializer(typeof(T));
            var setings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };
            var writer = XmlWriter.Create(_path, setings);
            serializerr.WriteObject(writer, data);
            writer.Close();
        }

        private bool IsValidExtention(string path)
        {
            int index = path.LastIndexOf('.');
            string extention = path.Substring(index + 1, path.Length - index - 1);
            if (extention == "xml")
            {
                return true;
            }
            else 
            {
                UsedDefaultPath.Invoke(_defautPath);
                return false;
            }
        }

        private string CreateFilePath(string path)
        {
            if (IsValidExtention(path))
            {
                IsCreateNewDirectory(path);
                return path;
            }
            else 
            {
                IsCreateNewDirectory(_defautPath);
                return _defautPath;
            }
        }

        private bool IsCreateNewDirectory(string path)
        {
            int index = path.LastIndexOf('/');
            string directory = path.Substring(0, index);
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
