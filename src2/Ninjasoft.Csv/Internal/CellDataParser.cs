using System;

namespace Ninjasoft.Csv.Internal
{
    internal interface ICellDataParser
    {
        object Parse(Type instancePropertyType, string cell, int lineNumber);
    }

    internal sealed class CellDataParser : ICellDataParser
    {
        public object Parse(Type instancePropertyType, string cell, int lineNumber)
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
                    return FindAndInvokeTryParse(instancePropertyType, cell, lineNumber);
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
                        return FindAndInvokeTryParse(instancePropertyType, cell, lineNumber);
                    break;
                default:
                    throw new InvalidOperationException($"{instancePropertyType.Name} not supported at this time");
            }

            return finalValue;
        }

        private object FindAndInvokeTryParse(Type instancePropertyType, string cell, int lineNumber)
        {
            var tryParseMethod = instancePropertyType.GetTryParseMethod();
            if (tryParseMethod.InvokeTryParseMethod(cell, out var finalValue))
                return finalValue;
            throw new InvalidOperationException($"Could not parse value '{cell}' to {instancePropertyType.Name} on line {lineNumber}");
        }
    }
}
