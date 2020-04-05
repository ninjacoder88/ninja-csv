using System;

namespace NinjaCsv.Internal.Interfaces
{
    public interface ICellDataParser
    {
        object Parse(Type instancePropertyType, string cell);
    }
}