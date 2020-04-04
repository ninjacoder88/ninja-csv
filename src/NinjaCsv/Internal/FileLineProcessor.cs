using System;
using System.Collections.Generic;
using System.Reflection;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv
{
    internal class FileLineProcessor : IFileLineProcessor
    {
        public object Process<T>(string fileLine, string delimiter, Dictionary<int, string> nameForPosition)
        {
            var instance = Activator.CreateInstance(typeof(T));
            var instanceType = instance.GetType();

            var splitFileLine = fileLine.Split(new[] { delimiter }, StringSplitOptions.None);

            for (var i = 0; i < splitFileLine.Length; i++)
            {
                var c = splitFileLine[i];

                if (!nameForPosition.ContainsKey(i))
                {
                    continue;
                }

                var value = nameForPosition[i];

                var instancePropertyInfo = instanceType.GetProperty(value, BindingFlags.Public | BindingFlags.Instance);
                if (instancePropertyInfo == null)
                {
                    continue;
                }

                var declaringType = instancePropertyInfo.PropertyType;
                object finalValue;

                //TODO: this can be improved, no but really
                if (declaringType == typeof(int))
                {
                    if (int.TryParse(c, out int intValue))
                    {
                        finalValue = intValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (declaringType == typeof(double))
                {
                    if (double.TryParse(c, out double doubleValue))
                    {
                        finalValue = doubleValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (declaringType == typeof(decimal))
                {
                    if (decimal.TryParse(c, out decimal decimalValue))
                    {
                        finalValue = decimalValue;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                //TODO: bool
                //TODO: long
                //TODO: see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types
                else
                {
                    finalValue = c;
                }

                instancePropertyInfo.SetValue(instance, finalValue);
            }

            return instance;
        }
    }
}