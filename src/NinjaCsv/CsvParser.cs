using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NinjaCsv
{
    public class CsvParser
    {
        public IEnumerable<T> Parse<T>(string filePath, CsvParserOptions options = default)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (filePath == string.Empty)
                throw new ArgumentException($"{nameof(filePath)} cannot be empty");

            if (!File.Exists(filePath))
                throw new ArgumentException($"The file path {filePath} does not exist");

            if (options == null)
            {
                options = new CsvParserOptions();
            }

            var fileLines = File.ReadAllLines(filePath).ToList();

            if (options.ContainsHeaderRow)
                fileLines = fileLines.Skip(1).ToList();

            var nameForPosition = new Dictionary<int, string>();

            var properties = typeof(T).GetProperties();

            foreach (var propertyInfo in properties)
            {
                //TODO: make sure its simple value type not reference types or structs, https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types#simple-types

                var attributes = propertyInfo.GetCustomAttributes(false);
                var attribute = attributes.FirstOrDefault();
                if (attribute == null)
                {
                    continue;
                }

                var column = attribute as Column;
                //TODO: check for null?
                nameForPosition.Add(column.Position, propertyInfo.Name);
            }

            var items = new List<T>();
            for (var l = 0; l < fileLines.Count; l++)
            {
                var fileLine = fileLines[l];
                var splitFileLine = fileLine.Split(new[] {options.Delimiter}, StringSplitOptions.None);

                var instance = Activator.CreateInstance(typeof(T));
                var instanceType = instance.GetType();

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

                items.Add((T)instance);
            }

            return items;
        }
    }
}