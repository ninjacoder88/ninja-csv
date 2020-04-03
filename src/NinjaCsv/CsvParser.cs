using System;
using System.IO;

namespace NinjaCsv
{
    public class CsvParser
    {
        private string _filePath;
        private string _delimiter;

        public CsvParser(string filePath, string delimiter = ",")
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if (!File.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");

            if(delimiter == null)
                throw new ArgumentNullException(nameof(delimiter));

            if(delimiter == string.Empty)
                throw new ArgumentException($"{nameof(delimiter)} cannot be empty");

            _filePath = filePath;
            _delimiter = delimiter;
        }
    }
}