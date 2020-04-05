using System.Collections.Generic;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IFileLineProcessor
    {
        object Process<T>(string fileLine, string delimiter, IReadOnlyDictionary<int, string> nameForPosition);
    }
}