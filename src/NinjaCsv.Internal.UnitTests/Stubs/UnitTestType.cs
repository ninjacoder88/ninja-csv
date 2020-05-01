using System;
using System.Globalization;
using System.Reflection;

namespace NinjaCsv.Internal.UnitTests.Stubs
{
    public class UnitTestType : Type
    {
        public override Assembly Assembly { get; }

        public override string AssemblyQualifiedName { get; }

        public override Type BaseType { get; }

        public override string FullName { get; }

        public override Guid GUID { get; }

        public override Module Module { get; }

        public override string Name { get; }

        public override string Namespace { get; }

        public override Type UnderlyingSystemType { get; }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return new ConstructorInfo[] { };
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[] { };
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[] { };
        }

        public override Type GetElementType()
        {
            return null;
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            return null;
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return new EventInfo[] { };
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return null;
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return new FieldInfo[] { };
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            return null;
        }

        public override Type[] GetInterfaces()
        {
            return new Type[] { };
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return new MemberInfo[] { };
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return new MethodInfo[] { };
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            return null;
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return new Type[] { };
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return new PropertyInfo[] { };
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture,
                                            string[] namedParameters)
        {
            return null;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            return TypeAttributes.AnsiClass;
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types,
                                                              ParameterModifier[] modifiers)
        {
            return null;
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types,
                                                    ParameterModifier[] modifiers)
        {
            return null;
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return null;
        }

        protected override bool HasElementTypeImpl()
        {
            return false;
        }

        protected override bool IsArrayImpl()
        {
            return false;
        }

        protected override bool IsByRefImpl()
        {
            return false;
        }

        protected override bool IsCOMObjectImpl()
        {
            return false;
        }

        protected override bool IsPointerImpl()
        {
            return false;
        }

        protected override bool IsPrimitiveImpl()
        {
            return false;
        }
    }
}