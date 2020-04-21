using System.Collections.Generic;
using System.Reflection;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IPropertyInfoToColumnMapper
    {
        IEnumerable<KeyValuePair<int, PropertyInfoView>> Map(PropertyInfo[] properties, bool considerNonPublic);
    }
}