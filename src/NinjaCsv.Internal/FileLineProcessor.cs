using System;
using System.Collections.Generic;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv.Internal
{
    internal class FileLineProcessor : IFileLineProcessor
    {
        public FileLineProcessor(ICellDataParser cellDataParser)
        {
            _cellDataParser = cellDataParser ?? throw new ArgumentNullException(nameof(cellDataParser));
        }

        public T Process<T>(string fileLine, string delimiter, Type targetType, IEnumerable<KeyValuePair<int, PropertyInfoView>> propertyMap)
        {
            if(fileLine == null)
                throw new ArgumentNullException(nameof(fileLine));

            var splitFileLine = fileLine.Split(new[] { delimiter }, StringSplitOptions.None);
            var rowLength = splitFileLine.Length;

            var instance = Activator.CreateInstance(targetType);

            foreach (var keyValuePair in propertyMap)
            {
                SideEffect(keyValuePair, rowLength, splitFileLine, instance);
            }

            return (T)instance;
        }

        private void SideEffect(KeyValuePair<int, PropertyInfoView> keyValuePair, int rowLength, string[] splitFileLine, object instance)
        {
            if (rowLength - 1 < keyValuePair.Key)
                return;

            var cell = splitFileLine[keyValuePair.Key];

            var parsedCellValue = _cellDataParser.Parse(keyValuePair.Value.PropertyType, cell);

            var setMethod = keyValuePair.Value.SetMethod;

            if (setMethod == null)
                return;

            setMethod.Invoke(instance, new[] { parsedCellValue });
        }

        private readonly ICellDataParser _cellDataParser;
    }
}