using Ninjasoft.Csv.Internal;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ninjasoft.Csv
{
    public class CsvReader
    {
        public CsvReader()
        {
            _fileLineProcessor = new FileLineProcessor(new CellDataParser());
            _propertyInfoToColumnMapper = new PropertyInfoToColumnMapper();
        }

        public IEnumerable<T> Read<T>(StreamReader streamReader, Action<CsvReaderOptions> optionsConfig = null)
        {
            var options = new CsvReaderOptions();
            optionsConfig?.Invoke(options);

            var targetType = typeof(T);

            var properties = options.ConsiderNonPublic
                                 ? targetType.GetPublicAndNonPublicInstanceProperties()
                                 : targetType.GetPublicInstanceProperties();

            var propertyMap = _propertyInfoToColumnMapper.Map(properties, options.ConsiderNonPublic);

            int lineNumber = 0;
            while(streamReader.Peek() >= 0)
            {
                var str = streamReader.ReadLine();

                if (lineNumber == 0 && options.ContainsHeaderRow)
                {
                    //grab and parse header row
                    lineNumber++;
                    continue;
                }

                lineNumber++;

                yield return _fileLineProcessor.Process<T>(str, options.Delimiter, targetType, propertyMap.ColumnMap, lineNumber);
            }
        }

        public IEnumerable<T> Read<T>(string filePath, Action<CsvReaderOptions> optionsConfig = null)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            using(var streamReader = new StreamReader(filePath))
            {
                return Read<T>(streamReader, optionsConfig);
            }
        }

        private readonly IFileLineProcessor _fileLineProcessor;
        private readonly IPropertyInfoToColumnMapper _propertyInfoToColumnMapper;
    }
}
