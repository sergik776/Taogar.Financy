using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        void Error<T>(string message, ConsoleColor color = ConsoleColor.Gray);
        void Warn<T>(string message, ConsoleColor color = ConsoleColor.Gray);
        void Info<T>(string message, ConsoleColor color = ConsoleColor.Gray);
    }
}
