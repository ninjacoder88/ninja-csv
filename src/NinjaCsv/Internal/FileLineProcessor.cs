using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv
{
    internal class FileLineProcessor : IFileLineProcessor
    {
        public FileLineProcessor(ICellDataParser cellDataParser)
        {
            _cellDataParser = cellDataParser ?? throw new ArgumentNullException(nameof(cellDataParser));
        }

        public object Process<T>(string fileLine, string delimiter, IReadOnlyDictionary<int, string> nameForPosition)
        {
            if (string.IsNullOrEmpty(fileLine))
                throw new ArgumentException($"{nameof(fileLine)} must have a non-empty value");

            if (string.IsNullOrEmpty(delimiter))
                throw new ArgumentException($"{nameof(delimiter)} must have a non-empty value");

            if (nameForPosition == null)
                throw new ArgumentNullException(nameof(nameForPosition));

            if (!nameForPosition.Any())
                throw new ArgumentException($"{nameof(nameForPosition)} cannot be empty");

            //create a new instance of the type
            var instance = Activator.CreateInstance(typeof(T));

            //split the fileLine by the delimiter
            var splitFileLine = fileLine.Split(new[] {delimiter}, StringSplitOptions.None);

            //foreach fileLine
            for (var i = 0; i < splitFileLine.Length; i++)
            {
                //get the cell (value split by delimiter)
                var cell = splitFileLine[i];

                ParseCell(i, cell, instance, nameForPosition);
            }

            return instance;
        }

        private void ParseCell(int i, string cell, object instance, IReadOnlyDictionary<int, string> nameForPosition)
        {
            var instanceType = instance.GetType();

            if (!nameForPosition.TryGetValue(i, out string propertyName))
            {
                return;
            }

            var instancePropertyInfo = instanceType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (instancePropertyInfo == null)
            {
                //TODO: consider an exception here
                return;
            }

            var instancePropertyType = instancePropertyInfo.PropertyType;

            var finalValue = _cellDataParser.Parse(instancePropertyType, cell);

            var finalValueType = finalValue.GetType().FullName;
            if (instancePropertyType.FullName != finalValueType)
            {
                throw new InvalidOperationException($"The type of {propertyName} ({instancePropertyType.FullName}) does not match the parsed value of {finalValue} ({finalValueType})");
            }

            instancePropertyInfo.SetValue(instance, finalValue);
        }

        private readonly ICellDataParser _cellDataParser;
    }
}