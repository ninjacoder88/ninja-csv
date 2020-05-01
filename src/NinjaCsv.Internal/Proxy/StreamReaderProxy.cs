using System;
using System.IO;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv.Internal.Proxy
{
    internal class StreamReaderProxy : IStreamReader
    {
        public StreamReaderProxy(string filePath)
        {
            _streamReader = new StreamReader(filePath);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Peek()
        {
            return _streamReader.Peek();
        }

        public string ReadLine()
        {
            return _streamReader.ReadLine();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _streamReader.Dispose();

            _disposed = true;
        }

        private bool _disposed;
        private readonly StreamReader _streamReader;
    }
}