using System;
using System.Collections.Generic;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IFileLineProcessor
    {
        T Process<T>(string fileLine, string delimiter, Type targetType, IEnumerable<KeyValuePair<int, PropertyInfoView>> propertyMap);
    }
}