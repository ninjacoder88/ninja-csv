using System;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IStreamReader : IDisposable
    {
        int Peek();

        string ReadLine();
    }
}