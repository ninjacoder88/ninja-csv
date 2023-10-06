using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ninjasoft.Csv.Internal
{
    internal interface IPropertyInfoToColumnMapper
    {
        MappedProperties Map(PropertyInfo[] properties, bool considerNonPublic);
    }

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
                if (propertyInfoView.ColumnNumber > maxColumnNumber)
                    maxColumnNumber = propertyInfoView.ColumnNumber;
            }

            return new MappedProperties(columnMap, maxColumnNumber);
        }
    }
}
