using NinjaCsv.Internal;
using NinjaCsv.Internal.Extensions;
using NinjaCsv.Internal.Interfaces;
using NinjaCsv.Internal.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaCsv
{
    public interface ICsvCreator
    {
        void Create<T>(string filePath, string headerRow, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null);

        void Create<T>(string filePath, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null);
    }

    public class CsvCreator : ICsvCreator
    {
        public CsvCreator()
            : this(new PropertyInfoToColumnMapper(), s => new StreamWriterProxy(s))
        {
        }

        internal CsvCreator(IPropertyInfoToColumnMapper propertyInfoToColumnMapper, Func<string, IStreamWriter> streamWriterFactory)
        {
            _propertyInfoToColumnMapper = propertyInfoToColumnMapper;
            _streamWriterFactory = streamWriterFactory;
        }

        public void Create<T>(string filePath, string headerRow, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null)
        {
            var options = new CsvCreatorOptions();

            optionsConfig?.Invoke(options);

            var targetType = typeof(T);

            var properties = options.ConsiderNonPublic ? targetType.GetPublicAndNonPublicInstanceProperties() : targetType.GetPublicInstanceProperties();

            var propertyMap = _propertyInfoToColumnMapper.Map(properties, options.ConsiderNonPublic).OrderBy(x => x.Key).ToList();

            using (var sw = _streamWriterFactory(filePath))
            {
                if(!string.IsNullOrEmpty(headerRow))
                {
                    sw.Write(headerRow);
                    sw.WriteLine();
                }

                foreach (var item in items)
                {
                    for (int i = 0; i < propertyMap.Count; i++)
                    {
                        var property = propertyMap[i];
                        var getMethod = property.Value.GetMethod;
                        if (getMethod == null)
                            continue;

                        var value = getMethod.Invoke(item, null);

                        sw.Write(value);
                        if (i < propertyMap.Count - 1)
                            sw.Write(options.Delimiter);
                    }
                    sw.WriteLine();//todo: dont write if end of list
                }
            }
        }

        public void Create<T>(string filePath, IEnumerable<T> items, Action<CsvCreatorOptions> optionsConfig = null)
        {
            Create(filePath, items, optionsConfig);
        }

        private readonly IPropertyInfoToColumnMapper _propertyInfoToColumnMapper;
        private readonly Func<string, IStreamWriter> _streamWriterFactory;
    }
}
