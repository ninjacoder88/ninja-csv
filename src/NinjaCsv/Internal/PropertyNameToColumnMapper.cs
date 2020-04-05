using System;
using System.Collections.Generic;
using System.Reflection;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv
{
    internal class PropertyNameToColumnMapper : IPropertyNameToColumnMapper
    {
        public IEnumerable<KeyValuePair<int, string>> Map(PropertyInfo[] properties)
        {
            _ = properties ?? throw new ArgumentNullException(nameof(properties));

            //foreach property
            foreach (var propertyInfo in properties)
            {
                //get custom attributes on the property
                var columnAttributes = propertyInfo.GetCustomAttributes(false);

                int counter = 0;
                foreach (var columnAttribute in columnAttributes)
                {
                    if(counter > 0)
                        throw new InvalidOperationException($"{propertyInfo.Name} has multiple {nameof(Column)} attributes");
                    if (!(columnAttribute is Column column))
                        continue;
                    counter++;

                    //consider checking if propertyInfo.Name is null
                    yield return new KeyValuePair<int, string>(column.Position, propertyInfo.Name);
                }
            }
        }
    }
}