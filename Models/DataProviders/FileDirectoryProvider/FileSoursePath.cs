using System.IO;

namespace DelitaTrade.Models.DataProviders.FileDirectoryProvider
{
    public static class FileSoursePath
    {
        public static string GetFullFilePathExt(this string filePath)
        {
            FileInfo fileInfo = new($"../../../{filePath}");
            return fileInfo.FullName;
        }

        public static string GetFullFilePath(string filePath)
        {
            FileInfo fileInfo = new($"../../../{filePath}");
            return fileInfo.FullName;
        }
    }
}
