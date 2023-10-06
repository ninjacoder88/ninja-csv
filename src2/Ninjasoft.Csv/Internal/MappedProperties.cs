using System.Collections.Generic;

namespace Ninjasoft.Csv.Internal
{
    internal sealed class MappedProperties
    {
        public MappedProperties(Dictionary<int, PropertyInfoView> columnMap, int maxColumnNumber)
        {
            ColumnMap = columnMap;
            MaxColumnNumber = maxColumnNumber;
        }

        public Dictionary<int, PropertyInfoView> ColumnMap { get; }

        public int MaxColumnNumber { get; }
    }
}
