using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;

namespace DelitaTrade.Models.Loggers
{
    public class FileLogger : Logger
    {
        private readonly string _directory = "../../../Logger";
       
        public FileLogger()
        {
            if (Directory.Exists(_directory) == false)
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public override Logger Log(Exception exception, LogLevel logLevel)
        {
            Log(exception.Message, logLevel);

            using (StreamWriter writer = new StreamWriter($"{_directory}/{logLevel}.txt", true)) 
            {   
                writer.WriteLine(exception.ToString());
                writer.WriteLine($"Source - {exception.Source}\nData - {exception.Data}");
                writer.WriteLine($"StackTrace - {exception.StackTrace}\nInnerExceptioon - {exception.InnerException}");
                writer.WriteLine(new string('-', 50));
            }
            return new MessageBoxLogger();
        }
       

        public override Logger Log(string message, LogLevel logLevel)
        {
            using (StreamWriter writer = new StreamWriter($"{_directory}/{logLevel}.txt", true))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
            return new MessageBoxLogger();
        }
    }
}
