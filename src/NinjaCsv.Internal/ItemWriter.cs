using System.Collections.Generic;
using System.IO;

namespace NinjaCsv.Internal
{
    internal class ItemWriter
    {
        public void Write<T>(Dictionary<int, PropertyInfoView> columnMap, int maxColumnNumber, int columnNumber, StreamWriter sw, string delimiter, T item)
        {
            if (!columnMap.TryGetValue(columnNumber, out var propertyInfoView))
            {
                sw.Write(delimiter);
                return;
            }

            var getMethod = propertyInfoView.GetMethod;
            if (getMethod == null)
            {
                sw.Write(delimiter);
                return;
            }

            var value = getMethod.Invoke(item, null);
            sw.Write(value);

            if (columnNumber < maxColumnNumber - 1)
                sw.Write(delimiter);
        }
    }
}
