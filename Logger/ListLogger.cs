using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class ListLogger : ILogger
    {
        private List<ILogger> Loggers;
        public ListLogger(params ILogger[] loggers)
        {
            Loggers = new List<ILogger>(loggers);
        }

        public void Error<T>(string message)
        {
            foreach (var logger in Loggers)
            {
                logger?.Error<T>(message);
            }
        }

        public void Info<T>(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            foreach (var logger in Loggers)
            {
                logger?.Info<T>(message, color);
            }
        }

        public void Warn<T>(string message)
        {
            foreach (var logger in Loggers)
            {
                logger?.Warn<T>(message);
            }
        }
    }
}
