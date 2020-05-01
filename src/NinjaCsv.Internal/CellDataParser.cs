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
                    var tryParseMethod = instancePropertyType.GetTryParseMethod();
                    if (tryParseMethod.InvokeTryParseMethod(cell, out finalValue))
                        return finalValue;
                    else
                        throw new InvalidOperationException($"Could not parse value '{cell}' to {instancePropertyType.Name}");
                case TypeFullName.String:
                    finalValue = cell;
                    break;
                default:
                    throw new InvalidOperationException($"{instancePropertyType.Name} not supported at this time");
            }

            return finalValue;
        }
    }
}