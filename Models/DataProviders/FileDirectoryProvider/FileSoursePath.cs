using System.IO;

namespace DelitaTrade.Models.DataProviders.FileDirectoryProvider
{
    public static class FileSoursePath
    {
        public static string GetFullFilePath(this string filePath)
        {
            FileInfo fileInfo = new($"../../../{filePath}");
            return fileInfo.FullName;
        }
    }
}
