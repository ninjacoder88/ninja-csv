using System;
using System.Reflection;

namespace Ninjasoft.Csv.Internal
{
    internal sealed class PropertyClass
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
}
