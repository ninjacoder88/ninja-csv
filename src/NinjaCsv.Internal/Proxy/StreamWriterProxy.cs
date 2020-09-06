using System;
using System.IO;

namespace NinjaCsv.Internal.Proxy
{
    internal class StreamWriterProxy : IStreamWriter
    {
        public StreamWriterProxy(string filePath)
        {
            _streamWriter = new StreamWriter(filePath);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _streamWriter.Dispose();

            _disposed = true;
        }

        private bool _disposed;
        private StreamWriter _streamWriter;
    }
}