using System.IO;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface ISystemFile
    {
        bool Exists(string filePath);

        string[] ReadAllLines(string filePath);

        Stream OpenRead(string filePath);

        void CloseRead(Stream stream);
    }
}