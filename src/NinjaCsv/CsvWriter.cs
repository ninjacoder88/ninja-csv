using System;
using System.Collections.Generic;
using NinjaCsv.Internal;
using NinjaCsv.Internal.Interfaces;
using NinjaCsv.Internal.Proxy;

namespace NinjaCsv
{
    public class CsvWriter
    {
        public CsvWriter()
            : this(s => new StreamWriterProxy(s), new FileProxy())
        {
            
        }

        internal CsvWriter(Func<string, IStreamWriter> streamWriterFactory, IFile file)
        {
            _streamWriterFactory = streamWriterFactory ?? throw new ArgumentNullException(nameof(streamWriterFactory));
            _systemFile = file;
        }

        public void Write<T>(string filePath, IEnumerable<T> objects)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if (!_systemFile.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");
        }

        private Func<string, IStreamWriter> _streamWriterFactory;
        private readonly IFile _systemFile;
    }
}