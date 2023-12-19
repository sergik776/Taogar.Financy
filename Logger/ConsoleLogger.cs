using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Error<T>(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(DateTime.Now.ToString("dd.MM.yy HH:mm:ss:fff") + " |");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($" {typeof(T).Name}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" ERROR");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" | ");
            Console.WriteLine(message);
        }

        public void Warn<T>(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(DateTime.Now.ToString("dd.MM.yy HH:mm:ss:fff") + " |");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($" {typeof(T).Name}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" WARN ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" | ");
            Console.WriteLine(message);
        }

        public void Info<T>(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(DateTime.Now.ToString("dd.MM.yy HH:mm:ss:fff") + " |");
            Console.ForegroundColor = color;
            Console.Write($" {typeof(T).Name.PadRight(20).Substring(0, 20)}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" INFO");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" |");
            Console.WriteLine(message);
        }
    }
}
