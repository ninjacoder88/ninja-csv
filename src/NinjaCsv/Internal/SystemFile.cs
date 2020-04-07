using System.IO;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv
{
    internal class SystemFile : ISystemFile
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string[] ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }

        public void CloseRead(Stream stream)
        {
            stream.Close();
        }
    }
}