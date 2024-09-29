using System.IO;

namespace DelitaTrade.Models.DataProviders
{
    public class IdDataGenerator
    {
        const int minValue = 2;
        const int maxValue = 15;
        private string _idFilePath;
        private int _numberOfDigits;
        private int _encryptParametr;

        public IdDataGenerator(string idFilePath, int numberOfDigits, int encryptParametr)
        {
            _idFilePath = idFilePath;
            _numberOfDigits = numberOfDigits;
            _encryptParametr = encryptParametr;
        }

        public int NumberOfDigits 
        { 
            get => _numberOfDigits;
            private set
            {
                if (value < minValue && value > maxValue)
                {
                    throw new ArgumentOutOfRangeException($"Value must be betwen {minValue} and {maxValue}");
                }
                _numberOfDigits = value;
            }
        }

        public string GetId(int code)
        {
            if (code != _encryptParametr)
            {
                throw new InvalidOperationException("Invalid code!");
            }
            return GetId();
        }

        private string GetId()
        {
            long id = 0;
            if (File.Exists(_idFilePath))
            {
                DataFileEncryptProvider.EncryptDataBaseFile(_idFilePath, _encryptParametr);
                id = int.Parse(File.ReadAllText(_idFilePath));
            }
            else
            {
                id = 0;
            }

            id++;
            File.WriteAllText(_idFilePath, id.ToString());
            DataFileEncryptProvider.EncryptDataBaseFile(_idFilePath, _encryptParametr);
            return $"ПВ{id + GetNumberOfDigits()}";
        }

        private long GetNumberOfDigits()
        {
            return (long)Math.Pow(10, (double)(_numberOfDigits - 1));
        }
    }
}
