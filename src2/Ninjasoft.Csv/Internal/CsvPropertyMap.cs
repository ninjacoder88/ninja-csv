using System.Collections.Generic;

namespace Ninjasoft.Csv.Internal
{
    internal sealed class CsvPropertyMap : List<KeyValuePair<int, PropertyInfoView>>
    {
        public CsvPropertyMap(IEnumerable<KeyValuePair<int, PropertyInfoView>> list)
            : base(list)
        {
        }
    }
}
