using System;
using System.Collections.Generic;
using System.Reflection;
using NinjaCsv.Common;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv.Internal
{
    internal class PropertyInfoToColumnMapper : IPropertyInfoToColumnMapper
    {
        public IEnumerable<KeyValuePair<int, PropertyInfoView>> Map(PropertyInfo[] properties, bool considerNonPublic)
        {
            _ = properties ?? throw new ArgumentNullException(nameof(properties));

            //foreach property
            foreach (var propertyInfo in properties)
            {
                //get custom attributes on the property
                var customAttributes = propertyInfo.GetCustomAttributes(false);

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
                    var view = new PropertyInfoView(propertyInfo.Name, propertyInfo, considerNonPublic);
                    yield return new KeyValuePair<int, PropertyInfoView>(column.Position, view);
                }
            }
        }
    }
}