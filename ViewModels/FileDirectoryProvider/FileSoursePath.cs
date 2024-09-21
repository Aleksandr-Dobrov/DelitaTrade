using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.FileDirectoryProvider
{
    public static class FileSoursePath
    {        
        public static string GetFullFilePath(string filePath)
        {
            FileInfo fileInfo = new($"../../../{filePath}");
                        return fileInfo.FullName;
        }
    }
}
