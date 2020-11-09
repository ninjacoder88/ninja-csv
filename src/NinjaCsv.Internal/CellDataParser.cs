using System;
using NinjaCsv.Internal.Extensions;
using NinjaCsv.Internal.Interfaces;
    
namespace NinjaCsv.Internal
{
    internal static class TypeFullName
    {
        public const string Int32 = "System.Int32";
        public const string Double = "System.Double";
        public const string Decimal = "System.Decimal";
        public const string Int64 = "System.Int64";
        public const string Boolean = "System.Boolean";
        public const string String = "System.String";
        public const string DateTime = "System.DateTime";
        public const string NullableInt32 = "System.Nullable`1[[System.Int32, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]";
        public const string NullableDouble = "System.Nullable`1[[System.Double, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]";
        public const string NullableDecimal = "System.Nullable`1[[System.Decimal, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]";
        public const string NullableInt64 = "System.Nullable`1[[System.Int64, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]";
        public const string NullableBoolean = "System.Nullable`1[[System.Boolean, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]";
        public const string NullableDateTime = "System.Nullable`1[[System.DateTime, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]";
    }

    internal class CellDataParser : ICellDataParser
    {
        public object Parse(Type instancePropertyType, string cell)
        {
            _ = instancePropertyType ?? throw new ArgumentNullException(nameof(instancePropertyType));

            if (string.IsNullOrEmpty(cell))
            {
                return instancePropertyType.FullName == TypeFullName.String ? cell : Activator.CreateInstance(instancePropertyType);
            }

            object finalValue;
            //give credit to: https://stackoverflow.com/questions/569249/methodinfo-invoke-with-out-parameter
            switch (instancePropertyType.FullName)
            {
                case TypeFullName.Int32:
                case TypeFullName.Boolean:
                case TypeFullName.Decimal:
                case TypeFullName.Double:
                case TypeFullName.Int64:
                case TypeFullName.DateTime:
                    return FindAndInvokeTryParse(instancePropertyType, cell);
                case TypeFullName.String:
                    finalValue = cell;
                    break;
                case TypeFullName.NullableInt32:
                case TypeFullName.NullableBoolean:
                case TypeFullName.NullableDecimal:
                case TypeFullName.NullableDouble:
                case TypeFullName.NullableInt64:
                case TypeFullName.NullableDateTime:
                    if (cell.ToLower() == "null")
                        finalValue = null;
                    else
                        return FindAndInvokeTryParse(instancePropertyType, cell);
                    break;
                default:
                    throw new InvalidOperationException($"{instancePropertyType.Name} not supported at this time");
            }

            return finalValue;
        }

        private object FindAndInvokeTryParse(Type instancePropertyType, string cell)
        {
            var tryParseMethod = instancePropertyType.GetTryParseMethod();
            if (tryParseMethod.InvokeTryParseMethod(cell, out var finalValue))
                return finalValue;
            throw new InvalidOperationException($"Could not parse value '{cell}' to {instancePropertyType.Name}");
        }
    }
}