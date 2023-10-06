using Ninjasoft.Csv.Internal;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ninjasoft.Csv
{
    public interface ICsvCreator
    {
        void Create<T>(StreamWriter streamWriter, IEnumerable<T> items, Action<CsvWriterOptions> optionsConfig = null);

        void Create<T>(string filePath, IEnumerable<T> items, Action<CsvWriterOptions> optionsConfig = null);

        byte[] Create<T>(IEnumerable<T> items, Action<CsvWriterOptions> optionsConfig = null);
    }

    public class CsvWriter
    {
        public CsvWriter()
        {
            _propertyInfoToColumnMapper = new PropertyInfoToColumnMapper();
        }

        public void Create<T>(StreamWriter streamWriter, IEnumerable<T> items, Action<CsvWriterOptions> optionsConfig = null)
        {
            var options = new CsvWriterOptions();
            optionsConfig?.Invoke(options);

            var targetType = typeof(T);

            var properties = options.ConsiderNonPublic ?
                targetType.GetPublicAndNonPublicInstanceProperties() :
                targetType.GetPublicInstanceProperties();

            var mappedProperties = _propertyInfoToColumnMapper.Map(properties, options.ConsiderNonPublic);

            WriteToStream(options, mappedProperties, streamWriter, items);
        }

        public void Create<T>(string filePath, IEnumerable<T> items, Action<CsvWriterOptions> optionsConfig = null)
        {
            var options = new CsvWriterOptions();
            optionsConfig?.Invoke(options);

            using (var streamWriter = new StreamWriter(filePath))
            {
                Create(streamWriter, items, optionsConfig);
            }
        }

        public byte[] Create<T>(IEnumerable<T> items, Action<CsvWriterOptions> optionsConfig = null)
        {
            var options = new CsvWriterOptions();
            optionsConfig?.Invoke(options);

            byte[] array;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    Create(streamWriter, items, optionsConfig);
                }
                array = memoryStream.ToArray();
            }
            return array;
        }

        private void WriteHeaderRowFromPropertyMap(MappedProperties propertyMap, StreamWriter sw, string delimiter)
        {
            for (int i = 0; i <= propertyMap.MaxColumnNumber; i++)
            {
                if(!propertyMap.ColumnMap.TryGetValue(i, out var propertyInfoView))
                {
                    sw.Write(delimiter);
                    continue;
                }

                var headerName = propertyInfoView.HeaderName;

                if (string.IsNullOrEmpty(headerName))
                    headerName = propertyInfoView.PropertyName;

                sw.Write(headerName);
                if (i < propertyMap.MaxColumnNumber)
                    sw.Write(delimiter);
            }
            sw.WriteLine();
        }

        private void WriteProvidedHeaderRow(string headerRow, StreamWriter sw)
        {
            if (!string.IsNullOrEmpty(headerRow))
            {
                sw.Write(headerRow);
                sw.WriteLine();
            }
        }

        private void WriteItem<T>(MappedProperties mappedProperties, T item, StreamWriter sw, string delimiter)
        {
            for (int columnNumber = 0; columnNumber <= mappedProperties.MaxColumnNumber; columnNumber++)
            {
                if (!mappedProperties.ColumnMap.TryGetValue(columnNumber, out var propertyInfoView))
                {
                    sw.Write(delimiter);
                    continue;
                }

                var getMethod = propertyInfoView.GetMethod;
                if (getMethod == null)
                {
                    sw.Write(delimiter);
                    continue;
                }

                var value = getMethod.Invoke(item, null);
                sw.Write(value);

                if (columnNumber < mappedProperties.MaxColumnNumber)
                    sw.Write(delimiter);
            }

            sw.WriteLine();//todo: dont write if end of list
        }

        private void WriteToStream<T>(CsvWriterOptions options, MappedProperties mappedProperties, StreamWriter sw, IEnumerable<T> items)
        {
            if (options.HeaderRowOptions.AddHeaderRow)
            {
                if (options.HeaderRowOptions.UseHeaderNames)
                    WriteHeaderRowFromPropertyMap(mappedProperties, sw, options.Delimiter);
                else
                    WriteProvidedHeaderRow(options.HeaderRowOptions.HeaderRowText, sw);
            }

            foreach (var item in items)
            {
                WriteItem(mappedProperties, item, sw, options.Delimiter);
            }
        }

        private readonly IPropertyInfoToColumnMapper _propertyInfoToColumnMapper;
    }
}
