using NinjaCsv.Internal;
using NinjaCsv.Internal.Extensions;
using NinjaCsv.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaCsv
{
    public interface ICsvCreator
    {
        void Create<T>(StreamWriter streamWriter, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null);

        void Create<T>(string filePath, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null);

        byte[] Create<T>(IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null);
    }

    public class CsvCreator : ICsvCreator
    {
        public CsvCreator()
            : this(new PropertyInfoToColumnMapper())
        {
        }

        internal CsvCreator(IPropertyInfoToColumnMapper propertyInfoToColumnMapper)
        {
            _propertyInfoToColumnMapper = propertyInfoToColumnMapper;
        }

        public void Create<T>(StreamWriter streamWriter, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null)
        {
            var options = new CsvCreatorOptions();
            optionsConfig?.Invoke(options);

            var targetType = typeof(T);

            var properties = options.ConsiderNonPublic ?
                targetType.GetPublicAndNonPublicInstanceProperties() :
                targetType.GetPublicInstanceProperties();

            var mappedProperties = _propertyInfoToColumnMapper.Map(properties, options.ConsiderNonPublic);

            WriteToStream(options, mappedProperties, streamWriter, items);  
        }

        public void Create<T>(string filePath, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null)
        {
            using(var streamWriter = new StreamWriter(filePath))
            {
                Create(streamWriter, items, optionsConfig);
            }
        }

        public byte[] Create<T>(IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null)
        {
            byte[] array;
            using(var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    Create(streamWriter, items, optionsConfig);
                }
                array = memoryStream.ToArray();
            }
            return array;
        }

        private void WriteHeaderRowFromPropertyMap(CsvPropertyMap propertyMap, StreamWriter sw, string delimiter)
        {
            for (int i = 0; i < propertyMap.Count; i++)
            {
                var property = propertyMap[i];
                var headerName = property.Value.HeaderName;

                if (string.IsNullOrEmpty(headerName))
                    headerName = property.Value.PropertyName;

                sw.Write(headerName);
                if (i < propertyMap.Count - 1)
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
            //todo: consider column numbers
            for(int columnNumber = 0; columnNumber < mappedProperties.MaxColumnNumber; columnNumber++)
            {
                if (!mappedProperties.ColumnMap.TryGetValue(columnNumber, out var propertyInfoView))
                {
                    sw.Write(delimiter);
                    continue;
                }

                //var t = mappedProperties.ColumnMap[i];

                //var property = propertyMap[i];
                var getMethod = propertyInfoView.GetMethod;
                if (getMethod == null)
                {
                    sw.Write(delimiter);
                    continue;
                }

                var value = getMethod.Invoke(item, null);
                sw.Write(value);

                if (columnNumber < mappedProperties.MaxColumnNumber - 1)
                    sw.Write(delimiter);
            }

            //for (int i = 0; i < propertyMap.Count; i++)
            //{
            //    if (!propertyMap.TryGetValue(i, out var propertyInfoView))
            //        sw.WriteLine(delimiter);

            //    var property = propertyMap[i];
            //    var getMethod = property.GetMethod;
            //    if (getMethod == null)
            //        continue;

            //    var value = getMethod.Invoke(item, null);

            //    sw.Write(value);
            //    if (i < propertyMap.Count - 1)
            //        sw.Write(delimiter);
            //}
            sw.WriteLine();//todo: dont write if end of list
        }

        private void WriteToStream<T>(CsvCreatorOptions options, MappedProperties mappedProperties, StreamWriter sw, IEnumerable<T> items)
        {
            if(options.HeaderRowOptions.AddHeaderRow)
            {
                if(options.HeaderRowOptions.UseHeaderNames)
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
