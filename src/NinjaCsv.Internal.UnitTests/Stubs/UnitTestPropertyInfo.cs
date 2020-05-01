using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace NinjaCsv.Internal.UnitTests.Stubs
{
    public class UnitTestPropertyInfo : PropertyInfo
    {
        public UnitTestPropertyInfo()
        {
        }

        #region Customizations

        public void AddCustomAttribute(object customAttribute)
        {
            _customAttributes.Add(customAttribute);
        }

        public void CustomizeName(string propertyName)
        {
            _propertyName = propertyName;
        }

        public void CustomizePropertyType(Type propertyType)
        {
            _propertyType = propertyType;
        }

        public void CustomizeSetMethod(MethodInfo methodInfo)
        {
            CustomSetMethod = methodInfo;
        }

        private Type _propertyType;
        private readonly List<object> _customAttributes = new List<object>();
        private string _propertyName;

        #endregion

        public override PropertyAttributes Attributes { get; }

        public override bool CanRead { get; }

        public override bool CanWrite { get; }

        public override Type DeclaringType { get; }

        public override string Name => _propertyName;

        public override Type PropertyType => _propertyType;

        public override Type ReflectedType { get; }

        public MethodInfo CustomSetMethod { get; private set; }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return new MethodInfo[] { };
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return _customAttributes.ToArray();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return _customAttributes.ToArray();
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return null;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return new ParameterInfo[] { };
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return CustomSetMethod;
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
        }
    }
}