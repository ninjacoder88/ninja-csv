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
    }
}