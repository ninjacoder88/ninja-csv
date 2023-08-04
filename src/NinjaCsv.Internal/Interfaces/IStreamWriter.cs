using System;

namespace NinjaCsv.Internal.Interfaces
{
    internal interface IStreamWriter : IDisposable
    {
        void Write(object obj);

        void WriteLine();
    }
}
