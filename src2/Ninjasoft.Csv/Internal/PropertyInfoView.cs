using System;
using System.Reflection;

namespace Ninjasoft.Csv.Internal
{
    internal sealed class PropertyInfoView
    {
        public PropertyInfoView(string propertyName, PropertyInfo propertyInfo, bool considerNonPublic, int columnNumber, string headerName = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            PropertyType = propertyInfo.PropertyType;
            SetMethod = propertyInfo.GetSetMethod(considerNonPublic);
            GetMethod = propertyInfo.GetGetMethod(considerNonPublic);
            HeaderName = headerName;
            ColumnNumber = columnNumber;
        }

        public int ColumnNumber { get; }

        public string PropertyName { get; }

        public Type PropertyType { get; }

        public MethodInfo SetMethod { get; }

        public MethodInfo GetMethod { get; }

        public string HeaderName { get; }
    }
}
