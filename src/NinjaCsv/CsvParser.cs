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

        internal ICellDataParser CellDataParser
        {
            get => _cellDataParser ?? (_cellDataParser = new CellDataParser());
            set => _cellDataParser = value;
        }

        internal IFileLineProcessor FileLineProcessor
        {
            get => _fileLineProcessor ?? (_fileLineProcessor = new FileLineProcessor(CellDataParser));
            set => _fileLineProcessor = value;
        }

        internal IPropertyNameToColumnMapper PropertyNameToColumnMapper
        {
            get => _propertyNameToColumnMapper ?? (_propertyNameToColumnMapper = new PropertyNameToColumnMapper());
            set => _propertyNameToColumnMapper = value;
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

            if (!SystemFile.Exists(filePath))
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

            var nameForPosition = PropertyNameToColumnMapper.Map(properties);
            if (nameForPosition == null)
            {
                throw new InvalidOperationException("Failed to map"); //TODO: better exception
            }

            var nameForPositionDictionary = nameForPosition.ToDictionary(k => k.Key, v => v.Value);

            if (!nameForPositionDictionary.Any())
            {
                throw new InvalidOperationException("No column attributes were present");
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

        private ICellDataParser _cellDataParser;
        private IFileLineProcessor _fileLineProcessor;
        private IPropertyNameToColumnMapper _propertyNameToColumnMapper;
        private ISystemFile _systemFile;
    }
}