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
            _writer.Flush();
        }

        public void Log(string message)
        {
            try
            {
                _writer.WriteLine(message);
            }
            catch (ObjectDisposedException)
            { }
        }

        public void Dispose() => _writer.Dispose();
    }
}
