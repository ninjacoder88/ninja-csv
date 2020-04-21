using System.IO;
using NinjaCsv.Internal.Interfaces;

namespace NinjaCsv.Internal.Proxy
{
    internal class FileProxy : IFile
    {
        public bool Exists(string filePath)
        {
            return !string.IsNullOrEmpty(filePath) && File.Exists(filePath);
        }

        public string[] ReadAllLines(string filePath)
        {
            return string.IsNullOrEmpty(filePath) ? new string[0] : File.ReadAllLines(filePath);
        }
    }
}