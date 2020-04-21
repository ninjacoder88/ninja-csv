using System;
using System.Reflection;

namespace NinjaCsv.Internal.Extensions
{
    internal static class TypeExtensions
    {
        public static PropertyInfo[] GetPublicAndNonPublicInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static PropertyInfo[] GetPublicInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static MethodInfo GetTryParseMethod(this Type type)
        {
            return type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });
        }

        public static bool InvokeTryParseMethod(this MethodInfo methodInfo, string str, out object value)
        {
            value = null;
            object[] parameters = { str, null };
            object result = methodInfo.Invoke(null, parameters);
            bool bResult = (bool)result;
            if (bResult)
                value = parameters[1];
            return bResult;
        }
    }
}