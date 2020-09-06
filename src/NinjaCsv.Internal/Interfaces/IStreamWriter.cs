using System;

namespace NinjaCsv.Internal
{
    internal interface IStreamWriter : IDisposable
    {
        void WriteLine(string value);
    } 
}