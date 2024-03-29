﻿using NinjaCsv.Internal.Interfaces;
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

        public void Write(object obj)
        {
            _streamWriter.Write(obj);
        }

        public void WriteLine()
        {
            _streamWriter.WriteLine();
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
        private readonly StreamWriter _streamWriter;
    }
}
