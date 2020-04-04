namespace NinjaCsv.Internal.Interfaces
{
    internal interface ISystemFile
    {
        bool Exists(string filePath);

        string[] ReadAllLines(string filePath);
    }
}