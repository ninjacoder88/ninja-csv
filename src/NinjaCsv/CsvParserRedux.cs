using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NinjaCsv
{
    public static class FileProxy
    {
        public static FileStream OpenReadOnly(string path)
        {
            return File.Open(path, FileMode.Open, FileAccess.Read);
        }
    }

    public class CsvParserRedux
    {
        public static IEnumerable<T> Parse<T>(string filePath, CsvParserOptions options = null)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException($"{nameof(filePath)} must have a value");

            if (!File.Exists(filePath))
                throw new ArgumentException($"No file exists at {filePath}");

            var type = typeof(T);

            var properties = type.GetPublicAndNonPublicInstanceProperties();

            var dictionary = Map(properties);

            //read file
            FileStream fileStream = null;
            try
            {
                fileStream = FileProxy.OpenReadOnly(filePath);

                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;

                    //read line
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var splitLine = line.Split(new[] {options.Delimiter}, StringSplitOptions.None);

                        var instance = Activator.CreateInstance<T>();

                        //read cell
                        for (int i = 0; i < splitLine.Length; i++)
                        {
                            var cell = splitLine[i];

                            //if deserialization type has Column attribute with matching index
                            if (dictionary.ContainsKey(i))
                            {
                                //try parse
                                var entry = dictionary[i];

                                var propertyName = entry.Item1;
                                var propertyType = entry.Item2;

                                object finalValue = ParseCell(propertyType, cell);

                                instance.SetProperty(propertyName, finalValue);
                            }
                        }

                        yield return instance;
                    }
                }
            }
            finally
            {
                fileStream?.Close();
            }
        }

        private static Dictionary<int, Tuple<string, Type>> Map(PropertyInfo[] properties)
        {
            var dictionary = new Dictionary<int, Tuple<string, Type>>();

            //setup deserialization object
            foreach (var propertyInfo in properties)
            {
                var columnAttributes = propertyInfo.GetColumnAttributes();
                int counter = 1;
                foreach (var columnAttribute in columnAttributes)
                {
                    if (counter > 1)
                        throw new Exception("Property X contains multiple Column attributes");

                    if (dictionary.ContainsKey(columnAttribute.Position))
                        throw new Exception("Column with position X already exists");

                    dictionary.Add(columnAttribute.Position, new Tuple<string, Type>(propertyInfo.Name, propertyInfo.PropertyType));
                    counter++;
                }

                var setMethod = propertyInfo.GetPublicOrPrivateSetMethod();
                if (setMethod == null)
                    throw new Exception();
            }

            return dictionary;
        }

        private static object ParseCell(Type propertyType, string cell)
        {
            object finalValue;
            switch (propertyType.FullName)
            {
                case TypeFullName.Int32:
                case TypeFullName.Boolean:
                case TypeFullName.Decimal:
                case TypeFullName.Double:
                case TypeFullName.Int64:
                    var tryParseMethod = propertyType.GetTryParseMethod();
                    if (!tryParseMethod.InvokeTryParseMethod(cell, out finalValue))
                    {
                        throw new InvalidOperationException($"Could not parse {cell} to {propertyType.Name}");
                    }

                    break;
                case TypeFullName.String:
                    finalValue = cell;
                    break;
                default:
                    throw new InvalidOperationException($"{propertyType.Name} not supported at this time");
            }

            return finalValue;
        }
    }

    public static class PropertyInfoExtensions
    {
        public static IEnumerable<Column> GetColumnAttributes(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes<Column>();
        }

        public static MethodInfo GetPublicOrPrivateSetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetSetMethod(true);
        }
    }

    public static class TypeExtensions
    {
        public static PropertyInfo[] GetPublicAndNonPublicInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static MethodInfo GetTryParseMethod(this Type type)
        {
            return type.GetMethod("TryParse", new[] {typeof(string), type.MakeByRefType()});
        }

        public static bool InvokeTryParseMethod(this MethodInfo methodInfo, string str, out object value)
        {
            value = null;
            object[] parameters = {str, null};
            object result = methodInfo.Invoke(null, parameters);
            bool bResult = (bool) result;
            if (bResult)
                value = parameters[1];
            return bResult;
        }
    }

    public static class ObjectExtensions
    {
        public static void SetProperty(this object obj, string propertyName, object value)
        {
            var instanceProperty = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var setMethod = instanceProperty.GetPublicOrPrivateSetMethod();
            setMethod.Invoke(obj, new[] {value});
        }
    }
}