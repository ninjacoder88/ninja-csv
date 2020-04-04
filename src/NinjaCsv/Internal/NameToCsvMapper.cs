using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv
{
    internal class NameToCsvMapper : INameToCsvMapper
    {
        public IEnumerable<KeyValuePair<int, string>> Map(PropertyInfo[] properties)
        {
            //var nameForPosition = new Dictionary<int, string>();
            foreach (var propertyInfo in properties)
            {
                //TODO: make sure its simple value type not reference types or structs, https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types#simple-types

                var attributes = propertyInfo.GetCustomAttributes(false);
                var attribute = attributes.FirstOrDefault();
                if (attribute == null)
                {
                    continue;
                }

                var column = attribute as Column;
                //TODO: check for null?
                yield return new KeyValuePair<int, string>(column.Position, propertyInfo.Name);
                //nameForPosition.Add(column.Position, propertyInfo.Name);
            }
        }
    }
}