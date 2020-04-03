using System;
using System.IO;

namespace NinjaCsv
{
    public class CsvParser
    {
        private string _filePath;

        public CsvParser(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if(filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if(!File.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");

            _filePath = filePath;
        }
    }
}