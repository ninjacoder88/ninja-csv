using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaCsv
{
    public class CsvParser
    {
        public IEnumerable<T> Parse<T>(string filePath, CsvParserOptions options = default)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if (!File.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");

            if (options == null)
            {
                options = new CsvParserOptions();
            }

            return null;
        }
    }
}