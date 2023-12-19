using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logger
{
    public class FileLogger : ILogger
    {
        private readonly string logFilePath;

        public FileLogger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void Error<T>(string message)
        {
            LogToFile($"[ERROR] {typeof(T)}: {message}");
        }

        public void Warn<T>(string message)
        {
            LogToFile($"[WARNING] {typeof(T)}: {message}");
        }

        public void Info<T>(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            LogToFile($"[INFO] {typeof(T)}: {message}");
        }

        private void LogToFile(string logMessage)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine($"{DateTime.Now} - {logMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи в файл лога: {ex.Message}");
            }
        }
    }
}
