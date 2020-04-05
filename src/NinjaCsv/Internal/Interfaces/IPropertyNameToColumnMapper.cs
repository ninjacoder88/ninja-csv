using System.Collections.Generic;
using System.Reflection;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IPropertyNameToColumnMapper
    {
        IEnumerable<KeyValuePair<int, string>> Map(PropertyInfo[] properties);
    }
}