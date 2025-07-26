using System.Windows;

namespace DelitaTrade.Models.Loggers
{
    public class MessageBoxLogger : Logger
    {       
        public override Logger Log(Exception exception, LogLevel logLevel)
        {           
            MessageBox.Show($"{exception.Message}",logLevel.ToString(),MessageBoxButton.OK, (MessageBoxImage)logLevel);
            return new FileLogger();
        }

        public override Logger Log(string message, LogLevel logLevel)
        {           
            MessageBox.Show(message,logLevel.ToString(),MessageBoxButton.OK, (MessageBoxImage)logLevel);
            return new FileLogger();
        }
    }
}
