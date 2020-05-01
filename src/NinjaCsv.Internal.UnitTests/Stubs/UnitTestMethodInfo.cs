using System;
using System.Globalization;
using System.Reflection;

namespace NinjaCsv.Internal.UnitTests.Stubs
{
    public class UnitTestMethodInfo : MethodInfo
    {
        public override MethodAttributes Attributes { get; }

        public override Type DeclaringType { get; }

        public override RuntimeMethodHandle MethodHandle { get; }

        public override string Name { get; }

        public override Type ReflectedType { get; }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        public override MethodInfo GetBaseDefinition()
        {
            return null;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[] { };
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[] { };
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return MethodImplAttributes.IL;
        }

        public override ParameterInfo[] GetParameters()
        {
            return new ParameterInfo[] { };
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }
    }
}