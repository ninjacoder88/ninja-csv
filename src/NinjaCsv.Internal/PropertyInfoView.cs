using System;
using System.Reflection;

namespace NinjaCsv.Internal
{
    internal class PropertyInfoView
    {
        public PropertyInfoView(string propertyName, PropertyInfo propertyInfo, bool considerNonPublic)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            PropertyType = propertyInfo.PropertyType;
            SetMethod = propertyInfo.GetSetMethod(considerNonPublic);
        }

        public string PropertyName { get; }

        public Type PropertyType { get; }

        public MethodInfo SetMethod { get; }
    }
}