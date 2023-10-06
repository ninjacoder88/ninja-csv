using System.Collections.Generic;
using System.Reflection;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IPropertyInfoToColumnMapper
    {
        MappedProperties Map(PropertyInfo[] properties, bool considerNonPublic);
    }
}