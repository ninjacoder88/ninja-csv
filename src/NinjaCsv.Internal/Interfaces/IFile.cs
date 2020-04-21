namespace NinjaCsv.Internal.Interfaces
{
    internal interface IFile
    {
        bool Exists(string filePath);

        string[] ReadAllLines(string filePath);
    }
}