using System.IO;

namespace DelitaTrade.Models.DataProviders
{
    public class DataFileEncryptProvider
    {        
        public static void EncryptDataBaseFile(string filePath, int encryptParametr)
        {
            if (File.Exists(filePath))
            {
                using (FileStream encrypting = new FileStream(filePath, FileMode.Open))
                {
                    byte[] data = new byte[encrypting.Length];

                    encrypting.Read(data, 0, data.Length);

                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = (byte)(data[i] ^ encryptParametr);
                    }

                    encrypting.Seek(0, SeekOrigin.Begin);
                    encrypting.Write(data, 0, data.Length);
                }
            }
        }
    }
}
