using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.Loggers
{
    public abstract class Logger
    {
        public enum LogLevel
        {
            Error = 16,
            Warning = 48,
            Information = 64,
        }

        public abstract Logger Log(string message, LogLevel logLevel);
        public abstract Logger Log(Exception ex, LogLevel logLevel);                
    }
}
