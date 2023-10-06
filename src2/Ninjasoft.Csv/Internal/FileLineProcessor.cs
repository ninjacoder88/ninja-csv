using System;
using System.Collections.Generic;

namespace Ninjasoft.Csv.Internal
{
    internal interface IFileLineProcessor
    {
        T Process<T>(string fileLine, string delimiter, Type targetType, Dictionary<int, PropertyInfoView> propertyMap, int lineNumber);
    }

    internal sealed class FileLineProcessor : IFileLineProcessor
    {
        public FileLineProcessor(ICellDataParser cellDataParser)
        {
            _cellDataParser = cellDataParser ?? throw new ArgumentNullException(nameof(cellDataParser));
        }

        //public T Process<T>(string fileLine, string delimiter, Type targetType, IEnumerable<KeyValuePair<int, PropertyInfoView>> propertyMap, int lineNumber)
        //{
        //    if (fileLine == null)
        //        throw new ArgumentNullException(nameof(fileLine));

        //    var splitFileLine = fileLine.Split(new[] { delimiter }, StringSplitOptions.None);
        //    var rowLength = splitFileLine.Length;

        //    var instance = Activator.CreateInstance(targetType);

        //    foreach (var keyValuePair in propertyMap)
        //    {
        //        SideEffect(keyValuePair, rowLength, splitFileLine, instance, lineNumber);
        //    }

        //    return (T)instance;
        //}

        public T Process<T>(string fileLine, string delimiter, Type targetType, Dictionary<int, PropertyInfoView> propertyMap, int lineNumber)
        {
            if (fileLine == null)
                throw new ArgumentNullException(nameof(fileLine));

            var splitFileLine = fileLine.Split(new[] { delimiter }, StringSplitOptions.None);
            var rowLength = splitFileLine.Length;

            var instance = Activator.CreateInstance(targetType);

            foreach (var keyValuePair in propertyMap)
            {
                SideEffect(keyValuePair, rowLength, splitFileLine, instance, lineNumber);
            }

            return (T)instance;
        }

        private void SideEffect(KeyValuePair<int, PropertyInfoView> keyValuePair, int rowLength, string[] splitFileLine, object instance, int lineNumber)
        {
            if (rowLength - 1 < keyValuePair.Key)
                return;

            var cell = splitFileLine[keyValuePair.Key];

            var parsedCellValue = _cellDataParser.Parse(keyValuePair.Value.PropertyType, cell, lineNumber);

            var setMethod = keyValuePair.Value.SetMethod;

            if (setMethod == null)
                return;

            setMethod.Invoke(instance, new[] { parsedCellValue });
        }

        private readonly ICellDataParser _cellDataParser;
    }
}
