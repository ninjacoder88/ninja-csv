using System;
using System.Collections.Generic;
using System.Linq;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv
{
    public class CsvParser
    {
        public CsvParser()
        {
        }

        internal IFileLineProcessor FileLineProcessor
        {
            get => _fileLineProcessor ?? (_fileLineProcessor = new FileLineProcessor());
            set => _fileLineProcessor = value;
        }

        internal INameToCsvMapper NameToCsvMapper
        {
            get => _nameToCsvMapper ?? (_nameToCsvMapper = new NameToCsvMapper());
            set => _nameToCsvMapper = value;
        }

        internal ISystemFile SystemFile
        {
            get => _systemFile ?? (_systemFile = new SystemFile());
            set => _systemFile = value;
        }

        public IEnumerable<T> Parse<T>(string filePath, CsvParserOptions options = null)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if (!_systemFile.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");

            var fileLines = _systemFile.ReadAllLines(filePath);
            if (fileLines == null)
            {
                throw new InvalidOperationException($"Reading lines from {filePath} returned no results");
            }

            var fileLineList = fileLines.ToList();

            if (!fileLineList.Any())
            {
                return new List<T>();
            }

            if (options == null)
            {
                options = new CsvParserOptions();
            }

            if (options.ContainsHeaderRow)
                fileLineList = fileLineList.Skip(1).ToList();

            var properties = typeof(T).GetProperties();

            var nameForPosition = NameToCsvMapper.Map(properties);
            if (nameForPosition == null)
            {
                throw new InvalidOperationException("Failed to map");//TODO: better exception
            }

            var nameForPositionDictionary = nameForPosition.ToDictionary(k => k.Key, v => v.Value);

            if (!nameForPositionDictionary.Any())
            {
                //exception?
            }
            
            var items = new List<T>();
            for (var l = 0; l < fileLineList.Count; l++)
            {
                var fileLine = fileLineList[l];

                var instance = FileLineProcessor.Process<T>(fileLine, options.Delimiter, nameForPositionDictionary);
                //check for null?

                items.Add((T) instance);
            }

            return items;
        }

        private IFileLineProcessor _fileLineProcessor;
        private INameToCsvMapper _nameToCsvMapper;
        private ISystemFile _systemFile;
    }
}