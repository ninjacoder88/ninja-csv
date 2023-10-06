using System;
using System.Collections.Generic;
using System.Reflection;
using NinjaCsv.Common;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv.Internal
{
    internal sealed class PropertyInfoToColumnMapper : IPropertyInfoToColumnMapper
    {
        public MappedProperties Map(PropertyInfo[] properties, bool considerNonPublic)
        {
            _ = properties ?? throw new ArgumentNullException(nameof(properties));

            Dictionary<int, PropertyInfoView> columnMap = new Dictionary<int, PropertyInfoView>();

            int maxColumnNumber = int.MinValue;
            foreach (var propertyInfo in properties)
            {
                //get custom attributes on the property
                var customAttributes = propertyInfo.GetCustomAttributes(false);

                var propertyInfoView = new PropertyClass().GetPropertyInfoDetails(propertyInfo, customAttributes, considerNonPublic);

                if (propertyInfoView == null)
                    continue;

                if (columnMap.ContainsKey(propertyInfoView.ColumnNumber))
                    throw new InvalidOperationException($"Multiple properties are marked with column number {propertyInfoView.ColumnNumber}");

                columnMap.Add(propertyInfoView.ColumnNumber, propertyInfoView);
                if(propertyInfoView.ColumnNumber > maxColumnNumber)
                    maxColumnNumber = propertyInfoView.ColumnNumber;
            }

            return new MappedProperties(columnMap, maxColumnNumber);
        }
    }

    internal class MappedProperties
    {
        public MappedProperties(Dictionary<int, PropertyInfoView> columnMap, int maxColumnNumber)
        {
            ColumnMap = columnMap;
            MaxColumnNumber = maxColumnNumber;
        }

        public Dictionary<int, PropertyInfoView> ColumnMap { get; }

        public int MaxColumnNumber { get; }
    }

    internal class PropertyClass
    {
        public PropertyInfoView GetPropertyInfoDetails(PropertyInfo propertyInfo, object[] customAttributes, bool considerNonPublic)
        {
            int counter = 0;
            foreach (var customAttribute in customAttributes)
            {
                if (counter > 0)
                    throw new InvalidOperationException($"{propertyInfo.Name} has multiple {nameof(Column)} attributes");
                if (!(customAttribute is Column column))
                    continue;
                if (column.Position < 0)
                    throw new InvalidOperationException($"{propertyInfo.Name} has a position value less than 0");
                counter++;

                //consider checking if propertyInfo.Name is null
                return new PropertyInfoView(propertyInfo.Name, propertyInfo, considerNonPublic, column.Position, column.HeaderName);
                //yield return new KeyValuePair<int, PropertyInfoView>(column.Position, view);
                //return new KeyValuePair<int, PropertyInfoView>(column.Position, view);
            }
            return null;
        }
    }

    internal sealed class CsvPropertyMap : List<KeyValuePair<int, PropertyInfoView>>
    {
        public CsvPropertyMap(IEnumerable<KeyValuePair<int, PropertyInfoView>> list)
            : base(list)
        {
        }
    }
}