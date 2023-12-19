using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        void Error<T>(string message);
        void Warn<T>(string message);
        void Info<T>(string message, ConsoleColor color = ConsoleColor.Gray);
    }
}
