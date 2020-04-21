using System;
using System.Collections.Generic;
using System.Linq;
using NinjaCsv.Internal;
using NinjaCsv.Internal.Extensions;
using NinjaCsv.Internal.Interfaces;
using NinjaCsv.Internal.Proxy;

namespace NinjaCsv
{
    public class CsvParser
    {
        public CsvParser()
            : this(new PropertyInfoToColumnMapper(), s => new StreamReaderProxy(s), new FileProxy(),
                   new FileLineProcessor(new CellDataParser()))
        {
        }

        internal CsvParser(IPropertyInfoToColumnMapper propertyInfoToColumnMapper, Func<string, IStreamReader> streamReaderFactory, 
                           IFile file, IFileLineProcessor fileLineProcessor)
        {
            _propertyInfoToColumnMapper = propertyInfoToColumnMapper ?? throw new ArgumentNullException(nameof(propertyInfoToColumnMapper));
            _streamReaderFactory = streamReaderFactory ?? throw new ArgumentNullException(nameof(streamReaderFactory));
            _systemFile = file ?? throw new ArgumentNullException(nameof(file));
            _fileLineProcessor = fileLineProcessor ?? throw new ArgumentNullException(nameof(fileLineProcessor));
        }

        public IEnumerable<T> Parse<T>(string filePath, Action<CsvParserOptions> optionsConfig = null)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if (!_systemFile.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");

            var options = new CsvParserOptions();

            optionsConfig?.Invoke(options);

            var targetType = typeof(T);

            var properties = options.ConsiderNonPublic
                                 ? targetType.GetPublicAndNonPublicInstanceProperties()
                                 : targetType.GetPublicInstanceProperties();

            var propertyMap = _propertyInfoToColumnMapper.Map(properties, options.ConsiderNonPublic).ToList();

            int lineNumber = 0;
            using (var sr = _streamReaderFactory(filePath))
            {
                while (sr.Peek() >= 0)
                {
                    var str = sr.ReadLine();

                    if (lineNumber == 0 && options.ContainsHeaderRow)
                    {
                        //grab and parse header row
                        lineNumber++;
                        continue;
                    }

                    lineNumber++;

                    var instance = _fileLineProcessor.Process<T>(str, options.Delimiter, targetType, propertyMap);

                    yield return instance;
                }
            }
        }

        private readonly IFileLineProcessor _fileLineProcessor;
        private readonly IPropertyInfoToColumnMapper _propertyInfoToColumnMapper;
        private readonly Func<string, IStreamReader> _streamReaderFactory;
        private readonly IFile _systemFile;
    }
}