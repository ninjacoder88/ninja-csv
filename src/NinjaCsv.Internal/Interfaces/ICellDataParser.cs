using System;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface ICellDataParser
    {
        object Parse(Type instancePropertyType, string cell, int lineNumber);
    }
}