using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static DelitaTrade.Models.Loggers.Logger;

namespace DelitaTrade.Models.Loggers
{
    public class MessageBoxLogger : Logger
    {       
        public override Logger Log(Exception exception, LogLevel logLevel)
        {           
            MessageBox.Show($"{exception.Source} - {exception.Message}",logLevel.ToString(),MessageBoxButton.OK, (MessageBoxImage)logLevel);
            return new FileLogger();
        }

        public override Logger Log(string message, LogLevel logLevel)
        {           
            MessageBox.Show(message,logLevel.ToString(),MessageBoxButton.OK, (MessageBoxImage)logLevel);
            return new FileLogger();
        }
    }
}
