using System;
using System.IO;

namespace WpfEssentials.Services
{
    public class FileLogger : IDisposable, ILogger
    {
        private readonly StreamWriter _writer;

        public FileLogger(string filePath) 
        {
            _writer = File.AppendText(filePath);
        }

        public void Log(string message) => _writer.WriteLine(message);

        public void Dispose() => _writer.Dispose();

    }
}
